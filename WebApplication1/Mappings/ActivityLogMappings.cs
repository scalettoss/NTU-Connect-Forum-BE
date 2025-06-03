using AutoMapper;
using ForumBE.DTOs.ActivitiesLog;
using ForumBE.DTOs.Statistics;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class ActivityLogMappings : Profile
    {
        public ActivityLogMappings()
        {
            CreateMap<ActivityLog, ActivityLogResponseDto>();
            //.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
            CreateMap<ActivityLog, LatestActivityResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
                ;
            CreateMap<ActivityLogCreateRequestDto, ActivityLog>();
        }
    }
}
