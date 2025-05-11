using ForumBE.DTOs.Notifications;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Implementations;
using ForumBE.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpGet]
        public async Task<ResponseBase> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return ResponseBase.Success(notifications);
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            return ResponseBase.Success(notification);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost]
        public async Task<ResponseBase> CreateNotification([FromBody] NotificationCreateRequestDto input)
        {
            var isCreated = await _notificationService.CreateNotificationAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created notification failed.");
            }
            return ResponseBase.Success("Created notification successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateNotification(int id, [FromBody] NotificationUpdateRequestDto input)
        {
            var isUpdated = await _notificationService.UpdateNotificationAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update notification failed.");
            }
            return ResponseBase.Success("Update notification successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteNotification(int id)
        {
            var isDeleted = await _notificationService.DeleteNotificationAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete notification failed.");
            }
            return ResponseBase.Success("Delete notification successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpGet("user")]
        public async Task<ResponseBase> GetNotificationsByUserId()
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync();
            return ResponseBase.Success(notifications);
        }
    }
}
