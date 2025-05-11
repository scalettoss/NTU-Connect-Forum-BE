using ForumBE.Models;
using ForumBE.Repositories.Generics;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class WarningRepository : GenericRepository<Warning>, IWarningRepository
    {
        public WarningRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetCountByUserId(int userId)
        {
            var count = await _context.Warnings
                .Where(w => w.UserId == userId)
                .CountAsync();

            return count;
        }
    }
}
