using AutoMapper;
using ForumBE.DTOs.ActivitiesLog;
using ForumBE.DTOs.Exception;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;

        public ActivityLogService(IActivityLogRepository activityLogRepository, IMapper mapper, ClaimContext userContextService)
        {
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<ActivityLogResponseDto>> GetAllLogs()
        {
            var logs = await _activityLogRepository.GetAllAsync();
            var logDtos = _mapper.Map<IEnumerable<ActivityLogResponseDto>>(logs);

            return logDtos;
        }

        public async Task<IEnumerable<ActivityLogResponseDto>> GetLogsByUser(int UserId)
        {
            var logs = await _activityLogRepository.GetLogByUser(UserId);
            var logDtos = _mapper.Map<IEnumerable<ActivityLogResponseDto>>(logs);

            return logDtos;
        }
        public async Task<bool> CreateLog(ActivityLogCreateRequestDto input)
        {
            try
            {
                var userId = _userContextService.GetUserId();
                var log = _mapper.Map<ActivityLog>(input);
                log.CreatedAt = DateTime.UtcNow;
                log.UserId = userId;
                await _activityLogRepository.AddAsync(log);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteLog(int logId)
        {
            try
            {
                var log = await _activityLogRepository.GetByIdAsync(logId);
                if (log == null)
                {
                    throw new HandleException("Log not found!", 404);
                }
                await _activityLogRepository.DeleteAsync(log);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
