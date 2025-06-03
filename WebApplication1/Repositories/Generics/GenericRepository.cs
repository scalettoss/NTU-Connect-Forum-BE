using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForumBE.Repositories.Generics
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);
            return item;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> attachments)
        {
            await _dbSet.AddRangeAsync(attachments);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<PagedList<T>> GetAllPagesAsync(PaginationDto input)
        {
            var query = _dbSet
               .AsQueryable();
            return await PagedList<T>.CreateAsync(query, input.PageNumber, input.PageSize);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await _dbSet.CountAsync(predicate);
            return await _dbSet.CountAsync();
        }
    }
}
