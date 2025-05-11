using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Categories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<PagedList<Category>> GetAllPagesAsync(PaginationDto input)
        {
            var query = _context.Categories
                .Include(c => c.Posts)
                .Where( c => c.IsDeleted == false)
                .AsQueryable();
            return await PagedList<Category>.CreateAsync(query, input.PageNumber, input.PageSize);
        }

        public Task<Category> GetBySlugAsync(string slug)
        {
            var category = _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Slug == slug && c.IsDeleted == false);
            return category;
        }

        public async Task<bool> IsExistingCategoryName(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
            if (category != null)
            {
                return true;
            }
            return false;
        }
    }
}
