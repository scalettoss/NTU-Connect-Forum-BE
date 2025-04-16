using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
        Task<IEnumerable<ActivityLog>> GetLogByUser(int UserId);
    }
}
