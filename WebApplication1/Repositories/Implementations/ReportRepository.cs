using ForumBE.Models;
using ForumBE.Repositories.Generics;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsExistingReport(int userId, int? postId, int? commentId)
        {
            if (postId != null)
            {
                var post = await _context.Reports.AnyAsync(r => r.UserId == userId && r.PostId == postId);
                return post;
            }
            else if (commentId != null)
            {
                var comment = await _context.Reports.AnyAsync(r => r.UserId == userId && r.CommentId == commentId);
                return comment;
            }
            return false;
        }
    }
}
