using ForumBE.DTOs.SystemConfigs;

namespace ForumBE.Services.SystemConfigs
{
    public interface ISystemConfigService
    {
        Task<bool> SetAutoApproved(SystemConfigRequestDto input);
        Task<bool> GetAutoApproved();
    }
}
