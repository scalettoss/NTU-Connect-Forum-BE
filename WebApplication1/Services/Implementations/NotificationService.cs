using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Notifications;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, ClaimContext userContextService)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationMap = _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
            return notificationMap;
        }

        public async Task<NotificationResponseDto> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                throw new HandleException("Notification not found", 404);
            }
            var notificationMap = _mapper.Map<NotificationResponseDto>(notification);
            return notificationMap;
        }

        public async Task<bool> CreateNotificationAsync(NotificationCreateRequestDto input)
        {
            try
            {
                var userId = _userContextService.GetUserId();
                var notification = _mapper.Map<Notification>(input);
                notification.IsRead = false;
                notification.UserId = userId;
                notification.CreatedAt = DateTime.UtcNow;

                await _notificationRepository.AddAsync(notification);
                return true;
            }
            catch (HandleException ex)
            {
                throw new HandleException(ex.Message, ex.Status);
            }
            
        }

        public async Task<bool> UpdateNotificationAsync(int id, NotificationUpdateRequestDto input)
        {
            try
            {
                var userId = _userContextService.GetUserId();
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    throw new HandleException("Notification not found", 404);
                }
                if (notification.UserId != userId)
                {
                    throw new HandleException("You are not authorized to update this notification", 403);
                }
                _mapper.Map(input, notification);
                notification.IsRead = false;
                notification.UpdatedAt = DateTime.UtcNow;
                await _notificationRepository.UpdateAsync(notification);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(id);
                if (notification == null)
                {
                    throw new HandleException("Notification not found", 404);
                }
                await _notificationRepository.DeleteAsync(notification);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
