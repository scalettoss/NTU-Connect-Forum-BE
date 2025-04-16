using ForumBE.DTOs.Warnings;

namespace ForumBE.Services.Interfaces
{
    public interface IWarningService
    {
        Task<List<WarningResponseDto>> GetAllWarningsAsync();
        Task<int> GetWarningsCountByUserId(int userId);
        Task<bool> CreateWarningAsync(WarningCreateRequestDto input);
        Task<bool> UpdateWarningAsync(int id, WarningUpdateRequestDto input);
        Task<bool> DeleteWarningAsync(int id);
    }
}
