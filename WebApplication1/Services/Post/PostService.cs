using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Categories;
using ForumBE.Repositories.Posts;

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
            ILogger<PostService> logger)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _claimContext = claimContext;
            _logger = logger;
        }

        public async Task<PagedList<PostResponseDto>> GetAllPostsAsync(PaginationDto input)
        {
            var userId = _claimContext.GetUserId();
            var posts = await _postRepository.GetAllPagesAsync(input);
            var postsDto = _mapper.Map<PagedList<PostResponseDto>>(posts);
            _logger.LogInformation("Fetching all posts.");
            return postsDto;

        }

        public async Task<PagedList<PostResponseDto>> GetAllPostByCategoryAsync(PaginationDto input, string slug)
        {
            var userId = _claimContext.GetUserId();
            var existingCategory = await _categoryRepository.GetBySlugAsync(slug);
            if (existingCategory == null)
            {
                throw new HandleException("Category not found!", 404);
            }
            var posts = await _postRepository.GetAllPagesByCategoryAsync(existingCategory.CategoryId, input);
            var postsDto = _mapper.Map<PagedList<PostResponseDto>>(posts);
            foreach (var post in postsDto.Items)
            {
                post.IsLiked = await _postRepository.HasUserLikedPostAsync(post.PostId, userId);
            }
            _logger.LogInformation("Fetching posts for category ID {CategoryId}.", existingCategory.CategoryId);
            return postsDto;
        }

        public async Task<PostResponseDto> GetPostByIdAsync(int postId)
        {
            var userId = _claimContext.GetUserId();
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new HandleException("Post not found!", 404);
            }
            var postDto = _mapper.Map<PostResponseDto>(post);
            var isLiked = await _postRepository.HasUserLikedPostAsync(postDto.PostId, userId);
            postDto.IsLiked = isLiked;
            _logger.LogInformation("Fetched post with ID {PostId}.", postId);
            return postDto;
        }

        public async Task<int> CreatePostAsync(PostCreateRequestDto request)
        {
            var userId = _claimContext.GetUserId();
            var slug = ConvertStringToSlug.ToSlug(request.Title);
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (existingCategory == null)
            {
                throw new HandleException("Category not found!", 404);
            }
            var postEntity = _mapper.Map<Models.Post>(request);
            postEntity.CreatedAt = DateTime.Now;
            postEntity.Status = "Pending";
            postEntity.UserId = userId;
            postEntity.Slug = slug;
            await _postRepository.AddAsync(postEntity);
            _logger.LogInformation("Post created successfully with ID {PostId} by user {UserId}.", postEntity.PostId, userId);
            return postEntity.PostId;
        }

        public async Task<bool> UpdatePostAsync(int postId, PostUpdateRequest request)
        {
            var userId = _claimContext.GetUserId();
            var userRole = _claimContext.GetUserRoleName();
            var existingPost = await _postRepository.GetByIdAsync(postId);
            if (existingPost == null)
            {
                throw new HandleException("Post not found!", 404);
            }

            var isOwner = existingPost.UserId == userId;
            var isAdminOrMod = userRole == "admin" || userRole == "moderator";
            if (!isOwner && !isAdminOrMod)
            {
                throw new HandleException("You are not authorized to update this post!", 401);
            }
            if (request.Title != null)
            {
                var slug = ConvertStringToSlug.ToSlug(request.Title);
                existingPost.Slug = slug;
            }
            _mapper.Map(request, existingPost);
            existingPost.UpdatedAt = DateTime.Now;
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
                throw new HandleException("Post not found!", 404);
            }

            var isOwner = post.UserId == userId;
            var isAdminOrMod = userRole == "admin" || userRole == "moderator";
            if (!isOwner && !isAdminOrMod)
            {
                throw new HandleException("You are not authorized to delete this post!", 403);
            }
            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);

            _logger.LogInformation("Post with ID {PostId} deleted successfully by user {UserId}.", postId, userId);
            return true;
        }

        public async Task<IEnumerable<PostResponseDto>> GetLatestPostsAsync(string? sortBy)
        {
            var userId = _claimContext.GetUserId();
            var latestPosts = await _postRepository.GetLatestPostsAsync(sortBy);
            var postDtos = _mapper.Map<IEnumerable<PostResponseDto>>(latestPosts);
            foreach (var post in postDtos)
            {
                post.IsLiked = await _postRepository.HasUserLikedPostAsync(post.PostId, userId);
            }
            _logger.LogInformation("Fetching latest posts");
            return postDtos;
        }

        public async Task<PostResponseDto> GetPostBySlugAsync(string slug)
        {
            var userId = _claimContext.GetUserId();
            var post = await _postRepository.GetPostBySlugAsync(slug);
            if (post == null)
            {
                throw new HandleException("Post not found!", 404);
            }
            var postDto = _mapper.Map<PostResponseDto>(post);
            var isLiked = await _postRepository.HasUserLikedPostAsync(postDto.PostId, userId);
            postDto.IsLiked = isLiked;
            _logger.LogInformation("Fetched post with Slug {slug}.", slug);
            return postDto;
        }
    }
}
