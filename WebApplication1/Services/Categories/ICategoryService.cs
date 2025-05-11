using ForumBE.DTOs.Categories;
using ForumBE.DTOs.Paginations;

namespace ForumBE.Services.Category
{
    public interface ICategoryService
    {
        Task<PagedList<CategoryResponseDto>> GetAllCategoriesAsync(PaginationDto input);
        Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId);
        Task<CategoryResponseDto> GetCategoryBySlugAsync(string slug);
        Task<bool> CreateCategoryAsync(CategoryCreateRequestDto input);
        Task<bool> UpdateCategoryAsync(int categoryId, CategoryUpdateRequestDto input);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
