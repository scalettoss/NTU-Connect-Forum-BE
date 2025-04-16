using Microsoft.EntityFrameworkCore;

namespace ForumBE.Helpers
{
    public static class PaginationExtension
    {
        public static async Task<PagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
}