using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Reports
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<bool> IsExistingReport(int userId, int? postId, int? commentId);
    }
}
