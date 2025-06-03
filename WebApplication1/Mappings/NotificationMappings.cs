using AutoMapper;
using ForumBE.DTOs.Notifications;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class NotificationMappings : Profile
    {
        public NotificationMappings()
        {
            CreateMap<Notification, NotificationResponseDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender != null ? src.Sender.FirstName + " " + src.Sender.LastName : "Hệ thống"))
                .ForMember(dest => dest.PostSlug, opt => opt.MapFrom(src => src.Post.Slug))
                .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Post.Category.Slug));
            CreateMap<NotificationCreateRequestDto, Notification>();
            CreateMap<NotificationUpdateRequestDto, Notification>();
        }
    }
}
