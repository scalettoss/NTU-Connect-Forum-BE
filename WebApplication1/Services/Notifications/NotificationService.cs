using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Notifications;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Notifications;
using ForumBE.Repositories.Posts;
using ForumBE.SignalR;

namespace ForumBE.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        private readonly ILogger<NotificationService> _logger;
        private readonly NotificationHub _notificationHub;

        public NotificationService(
            INotificationRepository notificationRepository,
            IMapper mapper,
            ClaimContext userContextService,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILogger<NotificationService> logger,
            NotificationHub notificationHub
            )
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _logger = logger;
            _notificationHub = notificationHub;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationMap = _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
            _logger.LogInformation("Fetching all notifications");
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
            _logger.LogInformation($"Fetching notification with ID: {id}");
            return notificationMap;
        }

        public async Task<bool> CreateNotificationAsync(NotificationCreateRequestDto input)
        {
            if (input.PostId is not null)
            {
                var post = await _postRepository.GetByIdAsync(input.PostId.Value);
                if (post == null)
                {
                    throw new HandleException("Post not found", 404);
                }
            }
            if (input.CommentId is not null)
            {
                var comment = await _commentRepository.GetByIdAsync(input.CommentId.Value);
                if (comment == null)
                {
                    throw new HandleException("Comment not found", 404);
                }
            }
            var userId = _userContextService.GetUserId();
            var notification = _mapper.Map<Notification>(input);
            notification.IsRead = false;
            notification.UserId = userId;
            notification.CreatedAt = DateTime.Now;

            await _notificationRepository.AddAsync(notification);
            await _notificationHub.SendNotificationToUser(notification.UserId, _mapper.Map<NotificationResponseDto>(notification));

            return true;
        }

        public async Task<bool> UpdateNotificationAsync(int id, NotificationUpdateRequestDto input)
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
            notification.UpdatedAt = DateTime.Now;
            await _notificationRepository.UpdateAsync(notification);
            return true;

        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {

            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                throw new HandleException("Notification not found", 404);
            }
            await _notificationRepository.DeleteAsync(notification);
            return true;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync()
        {
            var userId = _userContextService.GetUserId();
            var notifications = await _notificationRepository.GetByUserId(userId);
            var notificationsDto = _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
            return notificationsDto;
        }
    }
}
