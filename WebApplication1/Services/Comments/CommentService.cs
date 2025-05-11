using AutoMapper;
using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Interfaces;
using ForumBE.Repositories.Likes;
using ForumBE.Repositories.Posts;

namespace ForumBE.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CommentService> _logger;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ClaimContext _claimContext;

        public CommentService(
            ICommentRepository commentRepository,
            IMapper mapper,
            IPostRepository postRepository,
            ILogger<CommentService> logger,
            IAttachmentRepository attachmentRepository,
            ILikeRepository likeRepository,
            ClaimContext claimContext)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _claimContext = claimContext;
            _attachmentRepository = attachmentRepository;
            _likeRepository = likeRepository;
            _logger = logger;
        }

        public async Task<PagedList<CommentResponseDto>> GetAllCommentsAsync(PaginationDto input)
        {
            var userId = _claimContext.GetUserId();
            var comments = await _commentRepository.GetAllPagesAsync(input);
            var commentsDto = _mapper.Map<PagedList<CommentResponseDto>>(comments);
            foreach(var comment in commentsDto.Items)
{
                var isLiked = await _commentRepository.HasUserLikeComment(comment.CommentId, userId);
                comment.IsLiked = isLiked;
            }
            _logger.LogInformation("Fetching all comments.");
            return commentsDto;
        }

        public async Task<PagedList<CommentResponseDto>> GetAllCommentsByPostAsync(PaginationDto input, int postId)
        {
            var userId = _claimContext.GetUserId();
            var comments = await _commentRepository.GetAllCommentsByPost(input, postId);
            var commentsDto = _mapper.Map<PagedList<CommentResponseDto>>(comments);
            var commentIds = commentsDto.Items.Select(c => c.CommentId).ToList();
            var likedCommentIds = await _likeRepository.GetLikedCommentIdsByUserAsync(userId, commentIds);
            foreach (var comment in commentsDto.Items)
            {
                comment.IsLiked = likedCommentIds.Contains(comment.CommentId);
            }
            _logger.LogInformation("Fetching all comments for Post ID {PostId}.", postId);
            return commentsDto;
        }

        public async Task<CommentResponseDto> GetCommentByIdAsync(int commentId)
        {
            var userId = _claimContext.GetUserId();
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                throw new HandleException("Comment not found!", 404);
            }
            var commentsDto = _mapper.Map<CommentResponseDto>(comment);
            var likeCount = await _commentRepository.CountLike(commentsDto.CommentId);
            commentsDto.LikeCount = likeCount;
            var isLiked = await _commentRepository.HasUserLikeComment(commentsDto.CommentId, userId);
            commentsDto.IsLiked = isLiked;
            _logger.LogInformation("Fetching comment with ID {CommentId}.", commentId);
            return commentsDto;
        }

        public async Task<CommentResponseDto> CreateCommentAsync(CommentCreateRequestDto request)
        {
            var userId = _claimContext.GetUserId();

            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new HandleException("Post not found!", 404);
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                throw new HandleException("Comment content cannot be null or empty!", 400);
            }

            var comment = _mapper.Map<Comment>(request);
            if (request.ReplyTo != null)
            {
                var parentComment = await _commentRepository.GetByIdAsync((int)request.ReplyTo.Value);
                if (parentComment == null)
                {
                    throw new HandleException("Parent comment not found!", 404);
                }
                comment.ReplyTo = request.ReplyTo.Value;
            }
            comment.CreatedAt = DateTime.Now;
            comment.UserId = userId;
            await _commentRepository.AddAsync(comment);

            var commentsDto = _mapper.Map<CommentResponseDto>(comment);
            _logger.LogInformation("Successfully created comment for Post ID {PostId} by User ID {UserId}.", request.PostId, userId);
            return commentsDto;
        }

        public async Task<bool> UpdateCommentAsync(CommentUpdateRequestDto request, int commentId)
        {
            var currentUserId = _claimContext.GetUserId();
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                throw new HandleException("Comment not found!", 404);
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                throw new HandleException("Comment content cannot be null or empty!", 400);
            }

            if (comment.UserId != request.UserId)
            {
                throw new HandleException("Unauthorized update attempt on comment", 404);
            }

            _mapper.Map(request, comment);
            comment.UpdatedAt = DateTime.Now;
            await _commentRepository.UpdateAsync(comment);
            _logger.LogInformation("Successfully updated comment with ID {CommentId} by User ID {UserId}.", commentId);
            return true;
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                throw new HandleException("Comment not found!", 404);
            }
            if (userId != comment.UserId)
            {
                throw new HandleException("Unauthorized delete attempt on comment", 404);
            }

            if (comment.Attachments != null && comment.Attachments.Any())
            {
                await _attachmentRepository.DeleteRangeAsync(comment.Attachments);
            }

            comment.IsDeleted = true;
            await _commentRepository.UpdateAsync(comment);
            _logger.LogInformation("Successfully deleted comment with ID {CommentId} by User ID {UserId}.", commentId, userId);
            return true;
        }
    }
}
