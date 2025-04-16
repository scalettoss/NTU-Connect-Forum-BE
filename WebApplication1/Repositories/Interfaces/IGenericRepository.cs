using ForumBE.Helpers;

namespace ForumBE.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entit);
        Task DeleteAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<PagedResult<T>> GetPagedListAsync(int pageIndex, int pageSize);
    }
}
