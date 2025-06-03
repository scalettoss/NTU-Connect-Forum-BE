using AutoMapper;
using ForumBE.DTOs.UserProflies;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfile, UserProfileResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.User.CreatedAt))
                .ForMember(dest => dest.PostCount, opt => opt.MapFrom(src => src.User.Posts.Count(p => p.Status == "Approved" && p.IsDeleted == false)))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.User.Comments.Count(c => c.IsDeleted == false)))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.User.Likes.Count))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.User.Role.RoleName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive));

            CreateMap<UserProfileCreateRequestDto, UserProfile>();

            CreateMap<UserProfileUpdateRequestDto, UserProfile>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); ;
            CreateMap<UserProfileUpdateRequestDto, User>();
        }
    }
}
