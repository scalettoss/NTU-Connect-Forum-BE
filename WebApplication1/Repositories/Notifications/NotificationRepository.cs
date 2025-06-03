using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Notifications
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetByUserId(int userId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Post)
                .Include(n => n.Post.Category)
                .Where(n => n.UserId == userId && n.SenderId != userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(3)
                .ToListAsync();
            return notifications;
        }
    }
}
