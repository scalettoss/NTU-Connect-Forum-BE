using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Likes;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Likes;
using ForumBE.Repositories.Posts;
using ForumBE.Services.ActivitiesLog;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ClaimContext _userContextService;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public LikeService(
            ILikeRepository likeRepository, 
            ClaimContext userContextService, 
            IMapper mapper, 
            IPostRepository postRepository, 
            ICommentRepository commentRepository,
            IActivityLogService activityLogService)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
            _mapper = mapper;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _activityLogService = activityLogService;
        }
        public async Task<IEnumerable<LikeResponseDto>> GetAllLikesAsync()
        {
            var likes = await _likeRepository.GetAllAsync();
            var likesMap = _mapper.Map<IEnumerable<LikeResponseDto>>(likes);

            return likesMap;
        }

        public async Task<int> GetLikeCountFromPostAsync(int postId)
        {
            var count = await _likeRepository.GetLikeCountFromPostAsync(postId);

            return count;
        }
        public async Task<int> GetLikeCountFromCommentAsync(int commentId)
        {
            var count = await _likeRepository.GetLikeCountFromCommentAsync(commentId);

            return count;
        }
        public async Task<bool> ToggleLikeAsync(LikeToggleRequestDto input)
        {
            if (input.PostId == null && input.CommentId == null)
            {
                throw new HandleException("PostId or CommentId must be provided!", 400);
            }

            if (input.PostId != null && input.CommentId != null)
            {
                throw new HandleException("Only one of PostId or CommentId must be provided!", 400);
            }

            var userId = _userContextService.GetUserId();
            if (input.PostId != null)
            {
                var existingPost = await _postRepository.GetByIdAsync(input.PostId.Value);
                if (existingPost == null)
                {
                    throw new HandleException("Post not found!", 404);
                }
                var userLikePost = await _likeRepository.GetLikesPostByUser(input.PostId.Value, userId);
                if (userLikePost != null)
                {
                    await _likeRepository.DeleteAsync(userLikePost);
                    return true;
                }

                var like = _mapper.Map<Like>(input);
                like.PostId = input.PostId.Value;
                like.UserId = userId;
                like.CreatedAt = DateTime.Now;
                await _likeRepository.AddAsync(like);

                // Log like activity
                await ActivityLogHelper.LogActivityAsync(
                    _activityLogService,
                    ConstantString.ToggleLikeAction,
                    "Like",
                    $"Thích bài viết: {existingPost.Content}"
                );

                return true;
            }
            else if (input.CommentId != null)
            {
                var existingComment = await _commentRepository.GetByIdAsync(input.CommentId.Value);
                if (existingComment == null)
                {
                    throw new HandleException("Comment not found!", 404);
                }
                var userLikeComment = await _likeRepository.GetLikesCommentByUser(input.CommentId.Value, userId);
                if (userLikeComment != null)
                {
                    await _likeRepository.DeleteAsync(userLikeComment);
                    return true;
                }
                var like = _mapper.Map<Like>(input);
                like.CommentId = input.CommentId.Value;
                like.UserId = userId;
                like.CreatedAt = DateTime.Now;
                await _likeRepository.AddAsync(like);

                // Log like activity
                await ActivityLogHelper.LogActivityAsync(
                    _activityLogService,
                    ConstantString.ToggleLikeAction,
                    "Like",
                    $"Thích bình luận: {existingComment.Content}"
                );

                return true;
            }

            return false;
        }
    }
}
