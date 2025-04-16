using ForumBE.DTOs.ActivitiesLog;
using ForumBE.Response;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtivityLogController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        public AtivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }


        [HttpGet]
        public async Task<ResponseBase> GetAllActivityLog()
        {
            var activityLogs = await _activityLogService.GetAllLogs();
            return ResponseBase.Success(activityLogs);
        }

        [HttpGet("{userId}")]
        public async Task<ResponseBase> GetActivityLogByUser(int userId)
        {
            var activityLogs = await _activityLogService.GetLogsByUser(userId);
            return ResponseBase.Success(activityLogs);
        }
        [HttpPost]
        public async Task<ResponseBase> CreateActivityLog([FromBody] ActivityLogCreateRequestDto input)
        {
            var isCreated = await _activityLogService.CreateLog(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created activity log failed.");
            }
            return ResponseBase.Success("Created activity log successfully");
        }
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteActivityLog(int id)
        {
            var isDeleted = await _activityLogService.DeleteLog(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete activity log failed.");
            }
            return ResponseBase.Success("Delete activity log successfully");
        }
    }
}
