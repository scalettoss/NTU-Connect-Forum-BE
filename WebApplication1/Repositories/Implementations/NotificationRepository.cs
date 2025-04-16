using ForumBE.Models;
using ForumBE.Repositories.Interfaces;

namespace ForumBE.Repositories.Implementations
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
