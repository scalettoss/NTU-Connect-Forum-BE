using AutoMapper;
using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Exception;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ForumBE.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CommentService> _logger; 
        private readonly ClaimContext _claimContext;

        public CommentService(
            ICommentRepository commentRepository, 
            IMapper mapper, 
            IPostRepository postRepository,  
            ILogger<CommentService> logger,
            ClaimContext claimContext)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _claimContext = claimContext;
            _logger = logger;  
        }

        public async Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync()
        {
            _logger.LogInformation("Fetching all comments.");
            var list = await _commentRepository.GetAllAsync();
            var listMap = _mapper.Map<IEnumerable<CommentResponseDto>>(list);
            _logger.LogInformation("Successfully fetched all comments.");
            return listMap;
        }

        public async Task<CommentResponseDto> GetCommentByIdAsync(int commentId)
        {
            _logger.LogInformation("Fetching comment with ID {CommentId}.", commentId);
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                _logger.LogWarning("Comment with ID {CommentId} not found.", commentId);
                throw new HandleException("Comment not found!", 404);
            }
            _logger.LogInformation("Successfully fetched comment with ID {CommentId}.", commentId);
            var commentMap = _mapper.Map<CommentResponseDto>(comment);
            return commentMap;
        }

        public async Task<bool> CreateCommentAsync(CommentCreateRequestDto request)
        {
            var userId = _claimContext.GetUserId();

            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                _logger.LogWarning("Post with ID {PostId} not found when trying to create a comment.", request.PostId);
                throw new HandleException("Post not found!", 404);
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                _logger.LogWarning("Comment content is empty when trying to create a comment.");
                throw new HandleException("Comment content cannot be null or empty!", 400);
            }

            var comment = _mapper.Map<Comment>(request);
            comment.CreatedAt = DateTime.UtcNow;
            comment.UserId = userId;
            await _commentRepository.AddAsync(comment);

            _logger.LogInformation("Successfully created comment for Post ID {PostId} by User ID {UserId}.", request.PostId, userId);
            return true;
        }

        public async Task<bool> UpdateCommentAsync(int commentId, CommentUpdateRequestDto request)
        {
            var userId = _claimContext.GetUserId();
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                _logger.LogWarning("Comment with ID {CommentId} not found when trying to update.", commentId);
                throw new HandleException("Comment not found!", 404);
            }

            if (string.IsNullOrEmpty(request.Content))
            {
                _logger.LogWarning("Comment content is empty when trying to update comment with ID {CommentId}.", commentId);
                throw new HandleException("Comment content cannot be null or empty!", 400);
            }

            var userRole = _claimContext.GetUserRoleName();
            var isOwner = comment.UserId == userId;
            var isAdminOrMod = userRole == "admin" || userRole == "moderator";
            if (!isOwner && !isAdminOrMod)
            {
                _logger.LogWarning("Unauthorized update attempt on comment ID {PostId} by user {UserId}.", commentId, userId);
                throw new HandleException("Unauthorized update attempt on comment", 403);
            }

            _mapper.Map(request, comment);
            comment.UpdatedAt = DateTime.UtcNow;
            await _commentRepository.UpdateAsync(comment);

            _logger.LogInformation("Successfully updated comment with ID {CommentId} by User ID {UserId}.", commentId, userId);
            return true;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var userId = _claimContext.GetUserId();
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                _logger.LogWarning("Comment with ID {CommentId} not found when trying to delete.", commentId);
                throw new HandleException("Comment not found!", 404);
            }
            
            var userRole = _claimContext.GetUserRoleName();
            var isOwner = comment.UserId == userId;
            var isAdminOrMod = userRole == "admin" || userRole == "moderator";
            if (!isOwner && !isAdminOrMod)
            {
                _logger.LogWarning("Unauthorized delete attempt on comment ID {PostId} by user {UserId}.", commentId, userId);
                throw new HandleException("Unauthorized delete attempt on comment", 403);
            }

            await _commentRepository.DeleteAsync(comment);

            _logger.LogInformation("Successfully deleted comment with ID {CommentId} by User ID {UserId}.", commentId, userId);
            return true;
        }
    }
}
