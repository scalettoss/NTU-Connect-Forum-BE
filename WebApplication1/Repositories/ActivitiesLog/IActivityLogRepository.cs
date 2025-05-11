using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.ActivitiesLog
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
        Task<IEnumerable<ActivityLog>> GetLogByUser(int UserId);
    }
}
