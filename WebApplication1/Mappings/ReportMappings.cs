using AutoMapper;
using ForumBE.DTOs.Reports;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class ReportMappings : Profile
    {
        public ReportMappings()
        {
            CreateMap<Report, ReportResponseDto>();
            CreateMap<ReportCreateRequestDto, Report>();
            CreateMap<ReportUpdateRequestDto, Report>();
        }
    }
}
