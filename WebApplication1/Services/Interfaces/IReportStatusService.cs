using ForumBE.DTOs.ReportStatus;

namespace ForumBE.Services.Interfaces
{
    public interface IReportStatusService
    {
        Task<IEnumerable<ReportStatusResponseDto>> GetAllReportStatusesAsync();
        Task<ReportStatusResponseDto> GetReportStatusByIdAsync(int id);
        Task<bool> CreateReportStatusAsync(ReportStatusCreateRequestDto input);
        Task<bool> UpdateReportStatusAsync(int id, ReportStatusUpdateRequestDto input);
    }
}
