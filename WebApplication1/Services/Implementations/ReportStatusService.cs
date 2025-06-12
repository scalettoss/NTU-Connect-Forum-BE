using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.ReportStatus;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class ReportStatusService : IReportStatusService
    {
        private readonly IReportStatusRepository _reportStatusRepository;
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;

        public ReportStatusService(
            IReportStatusRepository reportStatusRepository,
            IReportService reportService,
            IMapper mapper,
            ClaimContext userContextService)
        {
            _reportStatusRepository = reportStatusRepository;
            _reportService = reportService;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<ReportStatusResponseDto>> GetAllReportStatusesAsync()
        {
            var reportStatuses = await _reportStatusRepository.GetAllAsync();
            var reportStatusesDto = _mapper.Map<IEnumerable<ReportStatusResponseDto>>(reportStatuses);
            return reportStatusesDto;
        }

        public async Task<ReportStatusResponseDto> GetReportStatusByIdAsync(int id)
        {
            var reportStatus = await _reportStatusRepository.GetByIdAsync(id);
            if (reportStatus == null)
            {
                throw new HandleException("ReportStatus not found.", 404);
            }
            var reportStatusDto = _mapper.Map<ReportStatusResponseDto>(reportStatus);
            return reportStatusDto;
        }

        public async Task<bool> CreateReportStatusAsync(ReportStatusCreateRequestDto input)
        {
            var existingReport = await _reportService.GetReportByIdAsync(input.ReportId);
            if (existingReport == null)
            {
                throw new HandleException("Report not found.", 404);
            }

            var existingReportStatus = await _reportStatusRepository.GetByIdAsync(input.ReportId);
            if (existingReportStatus != null)
            {
                throw new HandleException("ReportStatus already exists.", 400);
            }    
           
            var reportStatus = _mapper.Map<ReportStatus>(input);
            reportStatus.Status = "pending";
            reportStatus.IsProcessed = false;
            reportStatus.CreatedAt = DateTime.Now;
            reportStatus.HandledBy = null;

            await _reportStatusRepository.AddAsync(reportStatus);
            return true;
        }

        public async Task<bool> UpdateReportStatusAsync(int id, ReportStatusUpdateRequestDto input)
        {
            var userId = _userContextService.GetUserId();
            var reportStatus = await _reportStatusRepository.GetByIdAsync(id);
            if (reportStatus == null)
            {
                throw new HandleException("ReportStatus not found.", 404);
            }
            _mapper.Map(input, reportStatus);
            reportStatus.UpdatedAt = DateTime.Now;
            reportStatus.HandledBy = userId;
            reportStatus.IsProcessed = true;
            await _reportStatusRepository.UpdateAsync(reportStatus);

            return true;
        }


    }
}
