using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Interfaces
{
    public interface IReportStatusRepository : IGenericRepository<ReportStatus>
    {
        Task<ReportStatus> GetByReportIdAsync(int reportId);
    }
}
