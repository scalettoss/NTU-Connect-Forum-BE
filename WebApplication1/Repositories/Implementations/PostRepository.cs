using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class PostRepository : GenericRepository<Post> , IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Post>> GetAllByCategoryIdAsync(int categoryId, PaginationParams input)
        {
            var query = _context.Posts
                .Where(p => p.CategoryId == categoryId)
                .AsNoTracking(); 

            return await query.ToPagedListAsync(input.PageIndex, input.PageSize);
        }

        public async Task<PagedResult<Post>> GetAllWithUserPagedAsync(PaginationParams input)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .AsNoTracking();

            return await query.ToPagedListAsync(input.PageIndex, input.PageSize);
        }

        public async Task<Post> GetWithUserByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }
    }
}
