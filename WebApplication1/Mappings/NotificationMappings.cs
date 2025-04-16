using AutoMapper;
using ForumBE.DTOs.Notifications;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class NotificationMappings : Profile
    {
        public NotificationMappings()
        {
            CreateMap<Notification, NotificationResponseDto>();
            CreateMap<NotificationCreateRequestDto, Notification>();
            CreateMap<NotificationUpdateRequestDto, Notification>();
        }
    }
}
