using AutoMapper;
using ForumBE.DTOs.Categories;
using ForumBE.DTOs.Exception;
using ForumBE.Helpers;
using ForumBE.Repositories.Interfaces;

namespace ForumBE.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ClaimContext userContextService,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoriesResponseDto>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Getting all categories");
            var categoryList = await _categoryRepository.GetAllAsync();
            var categoryListMap = _mapper.Map<IEnumerable<CategoriesResponseDto>>(categoryList);
            return categoryListMap;
        }

        public async Task<CategoriesResponseDto> GetCategoryByIdAsync(int categoryId)
        {
            _logger.LogInformation("Getting category with ID: {CategoryId}", categoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", categoryId);
                throw new HandleException("Category not found!", 404);
            }
            var categoryMap = _mapper.Map<CategoriesResponseDto>(category);
            return categoryMap;
        }

        public async Task<bool> CreateCategoryAsync(CategoryCreateRequestDto request)
        {
            _logger.LogInformation("Creating new category with name: {CategoryName}", request.Name);
            var userId = _userContextService.GetUserId();
            var isExistingCategory = await _categoryRepository.IsExistingCategoryName(request.Name);
            if (isExistingCategory)
            {
                _logger.LogWarning("Category name {CategoryName} already exists", request.Name);
                throw new HandleException("Category name already exists!", 400);
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                _logger.LogWarning("Attempted to create a category with empty name");
                throw new HandleException("Category name cannot be null or empty!", 400);
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                _logger.LogWarning("Attempted to create a category with empty description");
                throw new HandleException("Category description cannot be null or empty!", 400);
            }

            var category = _mapper.Map<Models.Category>(request);
            category.CreatedAt = DateTime.UtcNow;
            category.UserId = userId;
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
                _logger.LogWarning("Category with ID {CategoryId} not found for update", categoryId);
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

            category.UpdatedAt = DateTime.UtcNow;
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
                _logger.LogWarning("Category with ID {CategoryId} not found for deletion", categoryId);
                throw new HandleException("Category not found!", 404);
            }
            await _categoryRepository.DeleteAsync(category);

            _logger.LogInformation("Category with ID {CategoryId} deleted successfully", categoryId);
            return true;
        }
    }
}
