using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;

namespace ForumBE.Repositories.Generics
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PagedList<T>> GetAllPagesAsync(PaginationDto input);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
