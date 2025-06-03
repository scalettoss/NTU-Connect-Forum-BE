using AutoMapper;
using ForumBE.DTOs.Statistics;
using ForumBE.Repositories.ActivitiesLog;
using ForumBE.Repositories.Attachments;
using ForumBE.Repositories.Categories;
using ForumBE.Repositories.Comments;
using ForumBE.Repositories.Posts;
using ForumBE.Repositories.Reports;
using ForumBE.Repositories.Users;

namespace ForumBE.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public StatisticsService(
            IUserRepository userRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ICategoryRepository categoryRepository,
            IReportRepository reportRepository,
            IAttachmentRepository attachmentRepository,
            IActivityLogRepository activityLogRepository,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _categoryRepository = categoryRepository;
            _reportRepository = reportRepository;
            _attachmentRepository = attachmentRepository;
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LatestActivityResponseDto>> GetLatestActivityAsync()
        {
            var latestActivity = await _activityLogRepository.GetLatestActivityAsync();
            var latestActivityDto = _mapper.Map<IEnumerable<LatestActivityResponseDto>>(latestActivity);
            return latestActivityDto;
        }

        public async Task<StatisticsResponseDto> GetStatisticsAsync()
        {
            var totalUsers = await _userRepository.CountAsync();
            var totalPosts = await _postRepository.CountAsync();
            var totalComments = await _commentRepository.CountAsync();
            var totalCategories = await _categoryRepository.CountAsync();
            var totalReports = await _reportRepository.CountAsync();
            var totalUploadFiles = await _attachmentRepository.CountAsync();

            return new StatisticsResponseDto
            {
                TotalUsers = totalUsers,
                TotalPosts = totalPosts,
                TotalComments = totalComments,
                TotalCategories = totalCategories,
                TotalReports = totalReports,
                TotalUploadFiles = totalUploadFiles
            };
        }
    }
}
