using ForumBE.DTOs.Notifications;
using ForumBE.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace ForumBE.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ClaimContext _claimContext;
        private readonly ILogger<NotificationHub> _logger;
        public NotificationHub(IHubContext<NotificationHub> hubContext, ClaimContext claimContext, ILogger<NotificationHub> logger)
        {
            _hubContext = hubContext;
            _claimContext = claimContext;
            _logger = logger;
        }
        public async Task SendNotificationToUser(int userId, NotificationResponseDto input)
        {
            await _hubContext.Clients.User(userId.ToString())
            .SendAsync("ReceiveNotification", input);
            _logger.LogInformation($"Notification sent to user {userId}: {input.Message}");
        }
    }
}
