using AutoMapper;
using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Attachments;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.IUnitOfWork;
using ForumBE.Repositories.Likes;
using ForumBE.Repositories.Posts;
using ForumBE.Services.ActivitiesLog;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ClaimContext _claimContext;
        private readonly IActivityLogService _activityLogService;

        public CommentService(
            ICommentRepository commentRepository,
            IMapper mapper,
            IPostRepository postRepository,
            ILogger<CommentService> logger,
            IAttachmentRepository attachmentRepository,
            ILikeRepository likeRepository,
            IUnitOfWork unitOfWork,
            ClaimContext claimContext,
            IActivityLogService activityLogService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _claimContext = claimContext;
            _attachmentRepository = attachmentRepository;
            _likeRepository = likeRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _activityLogService = activityLogService;
        }

        public async Task<PagedList<CommentResponseDto>> GetAllCommentsAsync(PaginationDto input)
        {
            var userId = _claimContext.GetUserId();
            var comments = await _commentRepository.GetAllPagesAsync(input);
            var commentsDto = _mapper.Map<PagedList<CommentResponseDto>>(comments);
            foreach (var comment in commentsDto.Items)
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
            var comments = await _commentRepository.GetAllCommentsByPost(input, postId, userId);
            //var commentsDto = _mapper.Map<PagedList<CommentResponseDto>>(comments);
            //var commentIds = commentsDto.Items.Select(c => c.CommentId).ToList();
            //var likedCommentIds = await _likeRepository.GetLikedCommentIdsByUserAsync(userId, commentIds);
            //foreach (var comment in commentsDto.Items)
            //{
            //    comment.IsLiked = likedCommentIds.Contains(comment.CommentId);
            //}
            _logger.LogInformation("Fetching all comments for Post ID {PostId}.", postId);
            return comments;
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
                throw new HandleException("Post not found!", 404);

            if (string.IsNullOrEmpty(request.Content))
                throw new HandleException("Comment content cannot be null or empty!", 400);

            await _unitOfWork.BeginTransactionAsync();

            var savedFilePaths = new List<string>();

            try
            {
                var comment = _mapper.Map<Comment>(request);
                comment.CreatedAt = DateTime.Now;
                comment.UserId = userId;

                if (request.ReplyTo != null)
                {
                    var parentComment = await _commentRepository.GetByIdAsync(request.ReplyTo.Value);
                    if (parentComment == null)
                        throw new HandleException("Parent comment not found!", 404);

                    comment.ReplyTo = request.ReplyTo.Value;
                }

                await _commentRepository.AddAsync(comment);

                // Xử lý file đính kèm nếu có
                if (request.Files != null && request.Files.Any())
                {
                    var attachments = await SaveAttachmentsAsync(request.Files, userId, comment.CommentId, savedFilePaths: savedFilePaths);

                    if (attachments.Any())
                    {
                        await _attachmentRepository.AddRangeAsync(attachments);
                    }
                    else
                    {
                        throw new HandleException("No valid files to upload.", 400);
                    }
                }

                // Log the activity
                await ActivityLogHelper.LogActivityAsync(
                    _activityLogService,
                    ConstantString.CreateCommentAction,
                    "Comment",
                    $"Bình luận vào bài viết: {post.Title}"
                );

                await _unitOfWork.CommitTransactionAsync();

                var commentsDto = _mapper.Map<CommentResponseDto>(comment);
                _logger.LogInformation("Successfully created comment for Post ID {PostId} by User ID {UserId}.", request.PostId, userId);
                return commentsDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                foreach (var filePath in savedFilePaths)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                throw new HandleException($"Create comment failed: {ex.Message}", 500);
            }
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

            // Log the activity
            await ActivityLogHelper.LogActivityAsync(
                _activityLogService,
                ConstantString.UpdateCommentAction,
                "Comment",
                $"Chỉnh sửa bình luận: {request.Content}"
            );

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

            // Log the activity
            await ActivityLogHelper.LogActivityAsync(
                _activityLogService,
                ConstantString.DeleteCommentAction,
                "Comment",
                $"Xóa bình luận: {comment.Content}"
            );

            _logger.LogInformation("Successfully deleted comment with ID {CommentId} by User ID {UserId}.", commentId, userId);
            return true;
        }

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> files, int userId, int commenId, List<string> savedFilePaths)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".txt", ".xlsx", ".pptx", ".mp3", ".gif" };
            const long MaxFileSize = 10 * 1024 * 1024;

            var dateFolder = DateTime.Now.ToString("yyyyMMdd");
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", dateFolder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var attachments = new List<Attachment>();

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                    throw new HandleException($"Invalid file extension for {file.FileName}.", 400);

                if (file.Length > MaxFileSize)
                    throw new HandleException($"File size exceeds 10MB for {file.FileName}.", 400);

                var uniqueFileName = Guid.NewGuid() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedFilePaths.Add(filePath);

                var fileUrl = $"/uploads/{dateFolder}/{uniqueFileName}";
                var fileSizeInMB = (file.Length / 1024f / 1024f).ToString("0.00") + " MB";

                attachments.Add(new Attachment
                {
                    CommentId = commenId,
                    UserId = userId,
                    FileUrl = fileUrl,
                    FileType = file.ContentType,
                    FileSize = fileSizeInMB,
                    CreatedAt = DateTime.Now,
                });
            }

            return attachments;
        }
    }
}
