using AutoMapper;
using Azure.Core;
using ForumBE.Controllers;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Reports;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Comments;
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
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        public ReportService(
            IReportRepository reportRepository, 
            IMapper mapper, 
            ClaimContext userContextService, 
            IPostRepository postRepository,
            IActivityLogService activityLogService,
            ICommentRepository commentRepository)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _postRepository = postRepository;
            _activityLogService = activityLogService;
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            var reportDtos = _mapper.Map<IEnumerable<ReportResponseDto>>(reports);
            return reportDtos;
        }

        public async Task<ReportResponseDto> GetReportByIdAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null)
            {
                throw new HandleException("Report not found", 404);
            }
            var reportDto = _mapper.Map<ReportResponseDto>(report);
            return reportDto;
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
                $"Báo cáo bài viết với lí do: {input.Reason}"
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
    }
}
