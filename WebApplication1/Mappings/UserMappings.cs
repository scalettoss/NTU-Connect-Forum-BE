using AutoMapper;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Users;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, UserResponseDto>()
                // get roll name from role
                .ForMember(dest => dest.RoleName,opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.UserProfile.AvatarUrl));
            CreateMap<UserUpdateProfilesRequestDto, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserUpdateProfilesRequestDto, UserProfile>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            
        }
    }
}
