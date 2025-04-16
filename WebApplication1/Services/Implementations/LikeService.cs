using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Likes;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
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
        public LikeService(ILikeRepository likeRepository, ClaimContext userContextService, IMapper mapper, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _likeRepository = likeRepository;
            _userContextService = userContextService;
            _mapper = mapper;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task<IEnumerable<LikeResponseDto>> GetAllLikesAsync()
        {
            var likes = await _likeRepository.GetAllAsync();
            var likesMap = _mapper.Map<IEnumerable<LikeResponseDto>>(likes);

            return likesMap;
        }
        public async Task<LikeResponseDto> GetLikeByIdAsync(int id)
        {
            var like = await _likeRepository.GetByIdAsync(id);
            if (like == null)
            {
                throw new HandleException("Like not found!", 404);
            }
            var likeMap = _mapper.Map<LikeResponseDto>(like);

            return likeMap;
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

            try
            {
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

                        if (!userLikePost.IsLike)
                        {
                            userLikePost.IsLike = true;
                        }
                        else
                        {
                            userLikePost.IsLike = false;
                        }
                        userLikePost.UpdatedAt = DateTime.UtcNow;
                        await _likeRepository.UpdateAsync(userLikePost);

                        return true;
                    }
                    var like = _mapper.Map<Like>(input);
                    like.PostId = input.PostId.Value;
                    like.UserId = userId;
                    like.IsLike = true;
                    like.CreatedAt = DateTime.UtcNow;
                    await _likeRepository.AddAsync(like);

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
                        if (!userLikeComment.IsLike)
                        {
                            userLikeComment.IsLike = true;
                        }
                        else
                        {
                            userLikeComment.IsLike = false;
                        }
                        userLikeComment.UpdatedAt = DateTime.UtcNow;
                        await _likeRepository.UpdateAsync(userLikeComment);

                        return true;
                    }
                    var like = _mapper.Map<Like>(input);
                    like.CommentId = input.CommentId.Value;
                    like.UserId = userId;
                    like.IsLike = true;
                    like.CreatedAt = DateTime.UtcNow;
                    await _likeRepository.AddAsync(like);

                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteLikeAsync(int id)
        {
            try
            {
                var like = await _likeRepository.GetByIdAsync(id);
                if (like == null)
                {
                    throw new HandleException("Like not found!", 404);
                }
                await _likeRepository.DeleteAsync(like);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
