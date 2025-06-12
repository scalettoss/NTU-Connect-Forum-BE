using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Reports;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Reports
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<bool> IsExistingReport(int userId, int? postId, int? commentId);
        Task<PagedList<ReportResponseDto>> GetAllPagedAsync(PaginationDto input);
        Task<ReportResponseDto> GetReportById(int id);
    }
}
