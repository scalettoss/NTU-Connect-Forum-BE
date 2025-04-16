using AutoMapper;
using ForumBE.DTOs.UserProflies;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfile, UserProfileResponseDto>();
            CreateMap<UserProfileCreateRequestDto, UserProfile>();
            CreateMap<UserProfileUpdateRequestDto, UserProfile>();
        }
    }
}
