using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {

        public ActivityLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ActivityLog>> GetLogByUser(int UserId)
        {
            var logs = await _context.ActivityLogs
                .Where(log => log.UserId == UserId)
                .ToListAsync();

            return logs;
        }
    }
}
