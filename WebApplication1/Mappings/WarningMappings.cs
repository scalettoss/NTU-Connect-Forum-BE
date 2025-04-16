using AutoMapper;
using ForumBE.DTOs.Warnings;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class WarningMappings : Profile
    {
        public WarningMappings()
        {
            CreateMap<Warning, WarningResponseDto>();
            CreateMap<WarningCreateRequestDto, Warning>();
            CreateMap<WarningUpdateRequestDto, Warning>();
        }
    }
}
