using AutoMapper;
using Azure.Core;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.DTOs.Reports;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Implementations;
using ForumBE.Repositories.Interfaces;
using ForumBE.Repositories.Posts;
using ForumBE.Repositories.Reports;
using ForumBE.Services.ActivitiesLog;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IActivityLogService _activityLogService;
        private readonly IReportStatusRepository _reportStatusRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        public ReportService(
            IReportRepository reportRepository, 
            IMapper mapper, 
            ClaimContext userContextService, 
            IPostRepository postRepository,
            IActivityLogService activityLogService,
            IReportStatusRepository reportStatusRepository,
            ICommentRepository commentRepository)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _postRepository = postRepository;
            _activityLogService = activityLogService;
            _commentRepository = commentRepository;
            _reportStatusRepository = reportStatusRepository;
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            var reportDtos = _mapper.Map<IEnumerable<ReportResponseDto>>(reports);
            return reportDtos;
        }

        public async Task<ReportResponseDto> GetReportByIdAsync(int id)
        {
            var report = await _reportRepository.GetReportById(id);
            if (report == null)
            {
                throw new HandleException("Report not found", 404);
            }
            return report;
        }

        public async Task<bool> CreateReportAsync(ReportCreateRequestDto input)
        {
            var userId = _userContextService.GetUserId();

            if (input.PostId == null && input.CommentId == null)
            {
                throw new HandleException("Either PostId or CommentId must be provided", 400);
            }

            if (input.PostId != null && input.CommentId != null)
            {
                throw new HandleException("Only one of PostId or CommentId must be provided", 400);
            }

            if (input.PostId != null)
            {
                var existingReport = await _reportRepository.IsExistingReport(userId, input.PostId, input.CommentId);
                if (existingReport)
                {
                    throw new HandleException("Bạn đã báo cáo bài viết này rồi", 400);
                }

                var existingPost = await _postRepository.GetByIdAsync(input.PostId.Value);
                if (existingPost == null)
                {
                    throw new HandleException("Post not found", 404);
                }
                await ActivityLogHelper.LogActivityAsync(
                _activityLogService,
                ConstantString.UpdateCommentAction,
                "Report",
                $"Báo cáo bài viết {existingPost.Title} với lí do: {input.Reason}"
                );
            }

            if (input.CommentId != null)
            {
                var existingReport = await _reportRepository.IsExistingReport(userId, input.PostId, input.CommentId);
                if (existingReport)
                {
                    throw new HandleException("You have already reported this comment", 400);
                }

                var existingComment = await _commentRepository.GetByIdAsync(input.CommentId.Value);
                if (existingComment == null)
                {
                    throw new HandleException("Comment not found", 404);
                }
                await ActivityLogHelper.LogActivityAsync(
                _activityLogService,
                ConstantString.UpdateCommentAction,
                "Report",
                $"Báo cáo bình luận với lí do: {input.Reason}");
            }
            var report = _mapper.Map<Report>(input);
            report.UserId = userId;
            report.CreatedAt = DateTime.Now;
            await _reportRepository.AddAsync(report);
            var reportStatus = new ReportStatus
            {
                ReportId = report.ReportId,
                Status = "pending",
                IsProcessed = false,
                CreatedAt = DateTime.Now,
                HandledBy = null,
                IsDeleted = false,
            };
            await _reportStatusRepository.AddAsync(reportStatus);

            return true;
        }

        public async Task<bool> UpdateReportAsync(int id, ReportUpdateRequestDto input)
        {
            var userId = _userContextService.GetUserId();

            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null)
            {
                throw new HandleException("Report not found", 404);
            }
            if (report.UserId != userId)
            {
                throw new HandleException("You are not authorized to update this report", 403);
            }
            _mapper.Map(input, report);
            report.UpdatedAt = DateTime.Now;
            await _reportRepository.UpdateAsync(report);

            return true;
        }

        public async Task<bool> DeleteReportAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null)
            {
                throw new HandleException("Report not found", 404);
            }
            await _reportRepository.DeleteAsync(report);
            return true;
        }

        public async Task<PagedList<ReportResponseDto>> GetAllReportPagedAsync(PaginationDto input)
        {
            var report = await _reportRepository.GetAllPagedAsync(input);
            return report;
        }

        public async Task<bool> HandelReportAsync(HandelReportRequestDto input)
        {
            var userId = _userContextService.GetUserId();
            var report = await _reportStatusRepository.GetByReportIdAsync(input.ReportId);
            if (report == null)
            {
                throw new HandleException("Report not found", 404);
            }

            if (input.Status != ConstantString.Rejected && input.Status != ConstantString.Resolved && input.Status != ConstantString.Pending)
            {
                throw new HandleException("Invalid status", 400);
            }
            report.Status = input.Status;
            report.UpdatedAt = DateTime.Now;
            report.HandledBy = userId;
            report.IsProcessed = true;
            await _reportStatusRepository.UpdateAsync(report);
            return true;
        }
    }
}
