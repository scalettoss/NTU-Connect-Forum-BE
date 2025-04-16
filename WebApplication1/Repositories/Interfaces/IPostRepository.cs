using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ForumBE.Repositories.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<PagedResult<Post>> GetAllWithUserPagedAsync(PaginationParams input);
        Task<PagedResult<Post>> GetAllByCategoryIdAsync(int categoryId, PaginationParams input);
        Task<Post> GetWithUserByIdAsync(int id);
    }
}
