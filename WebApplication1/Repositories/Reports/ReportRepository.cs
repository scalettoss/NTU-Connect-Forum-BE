using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Reports;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Reports
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedList<ReportResponseDto>> GetAllPagedAsync(PaginationDto input)
        {
            var query = _context.Reports.Select(p => new ReportResponseDto
            {
                ReportId = p.ReportId,
                PostId = p.Post.PostId,
                Title = p.Post.Title,
                Content = p.Post.Content,
                CreatedAt = p.CreatedAt,
                Status = p.ReportStatus.Status,
                FullName = p.User.FirstName + " " + p.User.LastName,
                AvatarUrl = p.User.UserProfile.AvatarUrl,
                Reason = p.Reason,
            }).AsQueryable();

            switch (input.SortBy?.ToLower())
            {
                case "approved":
                    query = query.Where(p => p.Status == ConstantString.Approved);
                    break;
                case "pending":
                    query = query.Where(p => p.Status == ConstantString.Pending);
                    break;
                case "rejected":
                    query = query.Where(p => p.Status == ConstantString.Rejected);
                    break;
            }

            return await PagedList<ReportResponseDto>.CreateAsync(query, input.PageNumber, input.PageSize);
        }

        public async Task<ReportResponseDto> GetReportById(int id)
        {
            var report = await _context.Reports.Select(p => new ReportResponseDto
            {
                ReportId = p.ReportId,
                PostId = p.Post.PostId,
                Title = p.Post.Title,
                Content = p.Post.Content,
                CreatedAt = p.CreatedAt,
                Status = p.ReportStatus.Status,
                FullName = p.User.FirstName + " " + p.User.LastName,
                AvatarUrl = p.User.UserProfile.AvatarUrl,
                Reason = p.Reason,
            }).FirstOrDefaultAsync(p => p.ReportId == id);

            return report;
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
