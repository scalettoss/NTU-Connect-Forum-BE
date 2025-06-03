using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Notifications;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Notifications;
using ForumBE.Repositories.Posts;
using ForumBE.Repositories.Users;
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
        private readonly IUserRepository _userRepository;
        private readonly NotificationHub _notificationHub;

        public NotificationService(
            INotificationRepository notificationRepository,
            IMapper mapper,
            ClaimContext userContextService,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILogger<NotificationService> logger,
            IUserRepository userRepository,
            NotificationHub notificationHub
            )
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _logger = logger;
            _userRepository = userRepository;
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
            Post post = null;
            if (input.PostId is not null)
            {
                post = await _postRepository.GetByIdAsync(input.PostId.Value);
                if (post == null)
                {
                    throw new HandleException("Post not found", 404);
                }
            }
            
            Comment comment = null;
            if (input.CommentId is not null)
            {
                comment = await _commentRepository.GetByIdAsync(input.CommentId.Value);
                if (comment == null)
                {
                    throw new HandleException("Comment not found", 404);
                }
            }

            var senderId = _userContextService.GetUserId();
            var sender = await _userRepository.GetByIdAsync(senderId);
            if (sender == null)
            {
                throw new HandleException("Sender not found", 404);
            }

            var fullName = sender.FirstName + " " + sender.LastName;
            // Tự động sinh message
            string message = GenerateMessage(input.Type, fullName, post, comment);

            // Tạo thông báo
            var notification = new Notification
            {
                UserId = input.UserId,            
                SenderId = senderId,              
                Type = input.Type,
                Message = message,
                PostId = input.PostId,
                CommentId = input.CommentId,
                IsRead = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _notificationRepository.AddAsync(notification);
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
        public async Task<bool> MarkReadNotification(MarkReadNotificationDto request)
        {
            var userId = _userContextService.GetUserId();
            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
            if (notification == null)
            {
                throw new HandleException("Notification not found", 404);
            }
            if (notification.UserId != userId)
            {
                throw new HandleException("You are not authorized to update this notification", 403);
            }
            notification.IsRead = true;
            notification.UpdatedAt = DateTime.Now;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        private string GenerateMessage(string type, string senderName, Post post, Comment comment)
        {
            return type switch
            {
                "Comment" => $"{senderName} đã bình luận vào bài viết của bạn.",
                "LikePost" => $"{senderName} đã thích bài viết của bạn.",
                "LikeComment" => $"{senderName} đã thích bình luận của bạn.",
                "Mention" => $"{senderName} đã nhắc đến bạn trong một bình luận.",
                "System" => "Bạn có một thông báo từ hệ thống.",
                _ => "Bạn có một thông báo mới."
            };
        }

        public async Task<bool> CreateSystemNotificationForAllUsersAsync(SystemNotificationRequestDto input)
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                throw new HandleException("No users found", 404);
            }

            var now = DateTime.Now;
            var notifications = users.Select(user => new Notification
            {
                UserId = user.UserId,
                SenderId = null,                 
                Type = "System",
                Message = input.Message,
                IsRead = false,
                CreatedAt = now,
                IsDeleted = false
            }).ToList();

            await _notificationRepository.AddRangeAsync(notifications); 
            return true;
        }

       
    }
}
