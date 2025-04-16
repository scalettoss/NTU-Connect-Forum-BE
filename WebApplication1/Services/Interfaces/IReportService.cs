using ForumBE.DTOs.Reports;

namespace ForumBE.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync();
        Task<ReportResponseDto> GetReportByIdAsync(int id);
        Task<bool> CreateReportAsync(ReportCreateRequestDto input);
        Task<bool> UpdateReportAsync(int id, ReportUpdateRequestDto input);
        Task<bool> DeleteReportAsync(int id);
    }
}
