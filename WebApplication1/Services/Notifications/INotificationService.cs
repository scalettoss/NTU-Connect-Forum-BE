using ForumBE.DTOs.Notifications;

namespace ForumBE.Services.Notifications
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<NotificationResponseDto> GetNotificationByIdAsync(int id);
        Task<bool> CreateNotificationAsync(NotificationCreateRequestDto input);
        Task<bool> UpdateNotificationAsync(int id, NotificationUpdateRequestDto input);
        Task<bool> DeleteNotificationAsync(int id);
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync();
    }
}
