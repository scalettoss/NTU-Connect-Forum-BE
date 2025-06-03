using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsExistingCategoryName(string name);
        Task<Category> GetBySlugAsync(string slug);
        Task<bool> IsSlugExistsAsync(string slug);

    }
}
