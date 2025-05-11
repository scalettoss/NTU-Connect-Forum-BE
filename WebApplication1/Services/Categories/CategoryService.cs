using AutoMapper;
using ForumBE.DTOs.Categories;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Repositories.Categories;
using ForumBE.Repositories.Posts;

namespace ForumBE.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IPostRepository postRepository,
            ClaimContext userContextService,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _postRepository = postRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<PagedList<CategoryResponseDto>> GetAllCategoriesAsync(PaginationDto input)
        {
            var categories = await _categoryRepository.GetAllPagesAsync(input);
            var categoriesDto = _mapper.Map<PagedList<CategoryResponseDto>>(categories);
            _logger.LogInformation("Getting all categories");
            return categoriesDto;
        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId)
        {
            _logger.LogInformation("Getting category with ID: {CategoryId}", categoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new HandleException("Category not found!", 404);
            }
            var categoryMap = _mapper.Map<CategoryResponseDto>(category);
            return categoryMap;
        }

        public async Task<bool> CreateCategoryAsync(CategoryCreateRequestDto request)
        {
            var userId = _userContextService.GetUserId();
            var slug = ConvertStringToSlug.ToSlug(request.Name);
            var isExistingCategory = await _categoryRepository.IsExistingCategoryName(request.Name);
            if (isExistingCategory)
            {
                throw new HandleException("Category name already exists!", 400);
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new HandleException("Category name cannot be null or empty!", 400);
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                throw new HandleException("Category description cannot be null or empty!", 400);
            }
            var category = _mapper.Map<Models.Category>(request);
            category.CreatedAt = DateTime.Now;
            category.UserId = userId;
            category.Slug = slug;
            await _categoryRepository.AddAsync(category);

            _logger.LogInformation("Category {CategoryName} created successfully", request.Name);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(int categoryId, CategoryUpdateRequestDto request)
        {
            _logger.LogInformation("Updating category with ID: {CategoryId}", categoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new HandleException("Category not found", 404);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                category.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                category.Description = request.Description;
            }

            category.UpdatedAt = DateTime.Now;
            await _categoryRepository.UpdateAsync(category);

            _logger.LogInformation("Category with ID {CategoryId} updated successfully", categoryId);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Deleting category with ID: {CategoryId}", categoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new HandleException("Category not found!", 404);
            }
            category.IsDeleted = true;
            await _categoryRepository.UpdateAsync(category);
            return true;
        }

        public async Task<CategoryResponseDto> GetCategoryBySlugAsync(string slug)
        {
            var category = await _categoryRepository.GetBySlugAsync(slug);
            if (category == null)
            {
                throw new HandleException("Category not found!", 404);
            }
            var categoryMap = _mapper.Map<CategoryResponseDto>(category);
            _logger.LogInformation("Getting category with Slug: {slug}", slug);
            return categoryMap;
        }
    }
}
