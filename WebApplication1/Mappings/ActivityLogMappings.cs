using AutoMapper;
using ForumBE.DTOs.ActivitiesLog;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class ActivityLogMappings : Profile
    {
        public ActivityLogMappings()
        {
            CreateMap<ActivityLog, ActivityLogResponseDto>();
            CreateMap<ActivityLogCreateRequestDto, ActivityLog>();
        }
    }
}
