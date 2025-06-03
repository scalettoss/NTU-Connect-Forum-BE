using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.ActivitiesLog
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {

        public ActivityLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ActivityLog>> GetLatestActivityAsync()
        {
            var latestActivity = await _context.ActivityLogs
                .Include(a => a.User)
                .OrderByDescending(log => log.CreatedAt)
                .Take(3)
                .ToListAsync();
            return latestActivity;
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
