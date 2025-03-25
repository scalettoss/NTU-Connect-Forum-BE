using AutoMapper;
using ForumBE.DTOs.Auth;
using ForumBE.DTOs.Users;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, GetUserResponseDto>()
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.UserProfiles.AvatarUrl))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.UserProfiles.Bio))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.UserProfiles.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.UserProfiles.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.UserProfiles.DateOfBirth))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.UserProfiles.Gender))
                .ForMember(dest => dest.LastLoginAt, opt => opt.MapFrom(src => src.UserProfiles.LastLoginAt))
                .ForMember(dest => dest.IsProfilePublic, opt => opt.MapFrom(src => src.UserProfiles.IsProfilePublic));
        }
    }
}
