using AutoMapper;
using ForumBE.DTOs.ReportStatus;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class ReportStatusMappings : Profile
    {
        public ReportStatusMappings()
        {
            CreateMap<ReportStatus, ReportStatusResponseDto>();
            CreateMap<ReportStatusCreateRequestDto, ReportStatus>();
            CreateMap<ReportStatusUpdateRequestDto, ReportStatus>();

        }
    }
}
