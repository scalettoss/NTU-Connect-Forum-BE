using ForumBE.DTOs.ActivitiesLog;
using ForumBE.Services.ActivitiesLog;

namespace ForumBE.Helpers
{
    public static class ActivityLogHelper
    {
        public static async Task LogActivityAsync(
            IActivityLogService activityLogService,
            string action,
            string module,
            string description = null)
        {
            var logEntry = new ActivityLogCreateRequestDto
            {
                Action = action,
                Module = module,
                Description = description ?? $"{action} operation on {module}"
            };

            await activityLogService.CreateLog(logEntry);
        }
    }
}