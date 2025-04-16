using ForumBE.DTOs.ActivitiesLog;
using ForumBE.Models;

namespace ForumBE.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<IEnumerable<ActivityLogResponseDto>> GetLogsByUser(int UserId);
        Task<IEnumerable<ActivityLogResponseDto>> GetAllLogs();
        Task<bool> CreateLog(ActivityLogCreateRequestDto input);
        Task<bool> DeleteLog(int logId);
    }
}
