using ForumBE.DTOs.Statistics;

namespace ForumBE.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<StatisticsResponseDto> GetStatisticsAsync();
        public Task<IEnumerable<LatestActivityResponseDto>> GetLatestActivityAsync();
    }
}
