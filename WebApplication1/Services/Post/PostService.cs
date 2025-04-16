using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;

namespace ForumBE.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;
        private readonly ClaimContext _claimContext;

        public PostService(
            IPostRepository postRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            ClaimContext claimContext,
            ILogger<PostService> logger) // injected ILogger<PostService>
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _claimContext = claimContext;
            _logger = logger;
        }

        public async Task<PaginationData<PostResponseDto>> GetAllPostsAsync(PaginationParams input)
        {
            var pagedPosts = await _postRepository.GetAllWithUserPagedAsync(input);
            _logger.LogInformation("Fetched {Count} posts.", pagedPosts.Items.Count());
            var postDtos = _mapper.Map<IEnumerable<PostResponseDto>>(pagedPosts.Items);

            var result = new PaginationData<PostResponseDto>
            {
                Data = postDtos,
                Pagination = new PagedResultDto
                {
                    TotalItems = pagedPosts.TotalItems,
                    PageIndex = pagedPosts.PageIndex,
                    PageSize = pagedPosts.PageSize,
                    TotalPages = input.PageSize > 0 ? (int)Math.Ceiling((double)pagedPosts.TotalItems / pagedPosts.PageSize) : 0
                }
            };

            return result;
        }

        public async Task<PostResponseDto> GetPostByIdAsync(int postId)
        {
            var post = await _postRepository.GetWithUserByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Post with ID {PostId} not found.", postId);
                throw new HandleException("Post not found!", 404);
            }

            _logger.LogInformation("Fetched post with ID {PostId}.", postId);
            var postDto = _mapper.Map<PostResponseDto>(post);
            return postDto;
        }

        public async Task<bool> CreatePostAsync(PostCreateRequestDto request)
        {
            var userId = _claimContext.GetUserId();
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (existingCategory == null)
            {
                _logger.LogWarning("Attempt to create post with non-existent category ID {CategoryId}.", request.CategoryId);
                throw new HandleException("Category not found!", 404);
            }

            var postEntity = _mapper.Map<Models.Post>(request);
            postEntity.CreatedAt = DateTime.UtcNow;
            postEntity.Status = "Pending";
            postEntity.UserId = userId;
            await _postRepository.AddAsync(postEntity);

            _logger.LogInformation("User {UserId} created a new post with title '{Title}'.", userId, request.Title);
            return true;
        }

        public async Task<bool> UpdatePostAsync(int postId, PostUpdateRequest request)
        {
            var userId = _claimContext.GetUserId();
            var userRole = _claimContext.GetUserRoleName();
            var existingPost = await _postRepository.GetByIdAsync(postId);
            if (existingPost == null)
            {
                _logger.LogWarning("Attempt to update non-existent post with ID {PostId}.", postId);
                throw new HandleException("Post not found!", 404);
            }

            var isOwner = existingPost.UserId == userId;
            var isAdminOrMod = userRole == "admin" || userRole == "moderator";
            if (!isOwner && !isAdminOrMod)
            {
                _logger.LogWarning("Unauthorized update attempt on post ID {PostId} by user {UserId}.", postId, userId);
                throw new HandleException("You are not authorized to update this post!", 403);
            }

            _mapper.Map(request, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;
            await _postRepository.UpdateAsync(existingPost);

            _logger.LogInformation("Post with ID {PostId} updated successfully by user {UserId}.", postId, userId);
            return true;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {

            var userId = _claimContext.GetUserId();
            var userRole = _claimContext.GetUserRoleName();
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogWarning("Attempt to delete non-existent post with ID {PostId}.", postId);
                throw new HandleException("Post not found!", 404);
            }

            var isOwner = post.UserId == userId;
            var isAdminOrMod = userRole == "admin" || userRole == "moderator";
            if (!isOwner && !isAdminOrMod)
            {
                _logger.LogWarning("Unauthorized delete attempt on post ID {PostId} by user {UserId}.", postId, userId);
                throw new HandleException("You are not authorized to delete this post!", 403);
            }

            await _postRepository.DeleteAsync(post);

            _logger.LogInformation("Post with ID {PostId} deleted successfully by user {UserId}.", postId, userId);
            return true;
        }

        public async Task<PaginationData<PostResponseDto>> GetAllPostByCategoryIdAsync(int categoryId, PaginationParams input)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
            {
                _logger.LogWarning("Attempt to fetch posts with non-existent category ID {CategoryId}.", categoryId);
                throw new HandleException("Category not found!", 404);
            }

            var pagedPosts = await _postRepository.GetAllByCategoryIdAsync(categoryId, input);
            var postDtos = _mapper.Map<IEnumerable<PostResponseDto>>(pagedPosts.Items);

            _logger.LogInformation("Fetched {Count} posts for category ID {CategoryId}.", postDtos.Count(), categoryId);

            var result = new PaginationData<PostResponseDto>
            {
                Data = postDtos,
                Pagination = new PagedResultDto
                {
                    TotalItems = pagedPosts.TotalItems,
                    PageIndex = pagedPosts.PageIndex,
                    PageSize = pagedPosts.PageSize,
                    TotalPages = input.PageSize > 0 ? (int)Math.Ceiling((double)pagedPosts.TotalItems / pagedPosts.PageSize) : 0
                }
            };

            return result;
        }
    }
}
