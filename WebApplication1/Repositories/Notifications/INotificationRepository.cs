using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Notifications
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserId(int userId);
    }
}
