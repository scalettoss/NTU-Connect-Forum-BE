using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<bool> IsExistingReport(int userId, int? postId, int? commentId);
    }
}
