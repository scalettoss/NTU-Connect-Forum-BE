using ForumBE.DTOs.Categories;
using ForumBE.DTOs.Users;
using ForumBE.Models;
using ForumBE.Repositories.Implementations;

namespace ForumBE.Services.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoriesResponseDto>> GetAllCategoriesAsync();
        Task<CategoriesResponseDto> GetCategoryByIdAsync(int categoryId);
        Task<bool> CreateCategoryAsync(CategoryCreateRequestDto request);
        Task<bool> UpdateCategoryAsync(int categoryId, CategoryUpdateRequestDto request);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
