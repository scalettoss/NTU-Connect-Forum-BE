using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Attachments;
using ForumBE.Repositories.Bookmarks;
using ForumBE.Repositories.Categories;
using ForumBE.Repositories.IUnitOfWork;
using ForumBE.Repositories.Posts;
using ForumBE.Repositories.ScamDetections;
using ForumBE.Repositories.SystemConfigs;
using ForumBE.Services.ActivitiesLog;
using ForumBE.Services.IPost;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using static ForumBE.DTOs.ScamDetections.ScamDetectionDto;


namespace ForumBE.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostService> _logger;
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ClaimContext _claimContext;
        private readonly IActivityLogService _activityLogService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IScamDetectionRepositoyry _scamDetectionRepositoyry;
        private readonly ISystemConfigRepository _systemConfigRepository;

        public PostService(
            IPostRepository postRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IBookmarkRepository bookmarkRepository,
            IAttachmentRepository attachmentRepository,
            ClaimContext claimContext,
            IUnitOfWork unitOfWork,
            ILogger<PostService> logger,
            IHttpClientFactory httpClientFactory,
            IActivityLogService activityLogService, 
            IScamDetectionRepositoyry scamDetectionRepositoyry,
            ISystemConfigRepository systemConfigRepository
            )
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _bookmarkRepository = bookmarkRepository;
            _attachmentRepository = attachmentRepository;
            _claimContext = claimContext;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _activityLogService = activityLogService;
            _httpClientFactory = httpClientFactory;
            _scamDetectionRepositoyry = scamDetectionRepositoyry;
            _systemConfigRepository = systemConfigRepository;
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
                post.IsBookmark = await _bookmarkRepository.IsAlreadyBookmark(post.PostId, userId);
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
            var baseSlug = ConvertStringToSlug.ToSlug(request.Title);
            var slug = await GenerateUniqueSlugAsync(baseSlug);
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (existingCategory == null)
                throw new HandleException("Category not found!", 404);

            await _unitOfWork.BeginTransactionAsync();

            var savedFilePaths = new List<string>();
            int postId = 0;

            try
            {
                // Tạo bài viết
                var postEntity = _mapper.Map<Models.Post>(request);
                postEntity.CreatedAt = DateTime.Now;
                postEntity.Status = "pending";
                postEntity.UserId = userId;
                postEntity.Slug = slug;

                await _postRepository.AddAsync(postEntity);
                postId = postEntity.PostId;

                // Nếu có file thì xử lý lưu file & đính kèm
                if (request.Files != null && request.Files.Any())
                {
                    var attachments = await SaveAttachmentsAsync(request.Files, userId, postEntity.PostId, savedFilePaths);

                    // Có file đính kèm hợp lệ thì lưu
                    if (attachments.Any())
                    {
                        await _attachmentRepository.AddRangeAsync(attachments);
                    }
                    // Có file nhưng tất cả đều không hợp lệ => rollback toàn bộ
                    else
                    {
                        throw new HandleException("No valid files to upload.", 400);
                    }
                }

                // Log the activity
                await ActivityLogHelper.LogActivityAsync(
                    _activityLogService,
                    ConstantString.CreatePostAction,
                    "Post",
                    $"Tạo bài viết với nội dung: {request.Title}"
                );

                var httpClient = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(new { content = request.Content }), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:5000/predict", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var prediction = JsonConvert.DeserializeObject<PredictionResponse>(responseContent);
                    float confidenceScore = prediction.data.ConfidenceScore;
                    bool modelPrediction = prediction.data.ModelPrediction;
                    string modelVersion = prediction.data.ModelVersion;
                    DateTime createdAt = prediction.data.CreatedAt;
                    var dataInsert = new ScamDetection
                    {
                        PostId = postId,
                        ConfidenceScore = confidenceScore,
                        ModelPrediction = modelPrediction,
                        ModelVersion = modelVersion,
                        CreatedAt = createdAt,
                        IsDeleted = false,
                    };
                    if (!modelPrediction)
                    {
                        var autoApproved = await _systemConfigRepository.GetByKeyAsync("AutoApproved");
                        if (autoApproved.Value && autoApproved.IsActive)
                        {
                            postEntity.Status = "approved";
                            await _postRepository.UpdateAsync(postEntity);
                        }
                    }
                    await _scamDetectionRepositoyry.AddAsync(dataInsert);
                }
                // Thành công
                await _unitOfWork.CommitTransactionAsync();
                return postEntity.PostId;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                // Xóa file vật lý nếu có
                foreach (var filePath in savedFilePaths)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                throw new HandleException($"Create post failed: {ex.Message}", 500);
            }
        }


        public async Task<bool> UpdatePostAsync(int postId, PostUpdateRequestDto request)
        {
            var userId = _claimContext.GetUserId();
            var userRole = _claimContext.GetUserRoleName();
            var existingPost = await _postRepository.GetByIdAsync(postId);
            if (existingPost == null)
            {
                throw new HandleException("Post not found!", 404);
            }

            if (request.Title != null)
            {
                var baseSlug = ConvertStringToSlug.ToSlug(request.Title);
                var slug = await GenerateUniqueSlugAsync(baseSlug);
                existingPost.Slug = slug;
            }
            _mapper.Map(request, existingPost);
            existingPost.UpdatedAt = DateTime.Now;
            await _postRepository.UpdateAsync(existingPost);

            // Log the activity
            await ActivityLogHelper.LogActivityAsync(
                _activityLogService,
                ConstantString.UpdatePostAction,
                "Post",
                $"Chỉnh sửa bài vi: {existingPost.Title}"
            );

            _logger.LogInformation("Post with ID {PostId} updated successfully by user {UserId}.", postId, userId);
            return true;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var userId = _claimContext.GetUserId();
            var userRole = _claimContext.GetUserRoleName();
            var post = await _postRepository.GetByIdAllStatusAsync(postId);
            if (post == null)
            {
                throw new HandleException("Post not found!", 404);
            }
            if (userRole == "admin")
            {
                post.IsDeleted = true;
                await _postRepository.UpdateAsync(post);
                return true;
            }
            if (userId != post.UserId)
            {
                throw new HandleException("You do not have permission to delete this post!", 403);
            }
            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);

            // Log the activity
            await ActivityLogHelper.LogActivityAsync(
                _activityLogService,
                ConstantString.DeletePostAction,
                "Post",
                $"Xóa bài viết: {post.Title}"
            );

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
            var isBookmark = await _bookmarkRepository.IsAlreadyBookmark(postDto.PostId, userId);
            postDto.IsLiked = isLiked;
            postDto.IsBookmark = isBookmark;
            _logger.LogInformation("Fetched post with Slug {slug}.", slug);
            return postDto;
        }

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> files, int userId, int postId, List<string> savedFilePaths)
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
                    PostId = postId,
                    UserId = userId,
                    FileUrl = fileUrl,
                    FileType = file.ContentType,
                    FileSize = fileSizeInMB,
                    CreatedAt = DateTime.Now,
                });
            }

            return attachments;
        }

        public Task<int> GetUserAuthorByPostAsync(int postId)
        {
            var authorId = _postRepository.GetUserAuthorByPostAsync(postId);
            if (authorId == null)
            {
                throw new HandleException("Post not found!", 404);
            }
            return authorId;
        }

        public async Task<string> GenerateUniqueSlugAsync(string baseSlug)
        {
            string slug = baseSlug;
            int counter = 1;

            while (await _postRepository.IsSlugExistsAsync(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public async Task<PagedList<PostAdminResponseDto>> GetAllPostByAdminAsync(PaginationDto input, PostSearchRequestDto condition)
        {
            var posts = await _postRepository.GetAllByAdmin(input, condition);
            _logger.LogInformation("Fetching all posts for admin.");
            return posts;
        }

        public async Task<PostAdminResponseDto> GetPostByAdminAsync(int postId)
        {
            var post = await _postRepository.GetPostByAdminAsync(postId);
            if (post == null)
            {
                throw new HandleException("Post not found!", 404);
            }
            _logger.LogInformation("Fetched post with ID {PostId} for admin.", postId);
            return post;
        }

        public async Task<bool> UpdatePostByAdminAsync(PostUpdateByAdminRequestDto input, int postId)
        {
            var post = await _postRepository.GetByIdAllStatusAsync(postId);
            if (post == null)
            {
                throw new HandleException("Post not found!", 404);
            }

            if (!string.IsNullOrEmpty(input.Title))
            {
                var baseSlug = ConvertStringToSlug.ToSlug(input.Title);
                var slug = await GenerateUniqueSlugAsync(baseSlug);
                post.Slug = slug;
                post.Title = input.Title;
            }

            if (!string.IsNullOrEmpty(input.Content))
            {
                post.Content = input.Content;
            }

            if (!string.IsNullOrEmpty(input.Status))
            {
                post.Status = input.Status;
            }
            post.UpdatedAt = DateTime.Now;
            await _postRepository.UpdateAsync(post);
            return true;
        }
    }
}
