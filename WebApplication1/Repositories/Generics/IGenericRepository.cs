using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using System.Linq.Expressions;

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
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
