
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.SystemConfigs;
using ForumBE.Repositories.SystemConfigs;

namespace ForumBE.Services.SystemConfigs
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _systemConfigRepository;
        public SystemConfigService(ISystemConfigRepository systemConfigRepository)
        {
            _systemConfigRepository = systemConfigRepository;
        }

        public async Task<bool> GetAutoApproved()
        {
            var autoApprovedConfig = await _systemConfigRepository.GetByKeyAsync("AutoApproved");
            if (autoApprovedConfig == null)
            {
                throw new HandleException("AutoApproved configuration not found.",404);
            }
            if (!autoApprovedConfig.IsActive)
            {
                throw new HandleException("AutoApproved configuration is not active.", 400);
            }
            if (autoApprovedConfig.Value)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> SetAutoApproved(SystemConfigRequestDto input)
        {
            var autoApprovedConfig = await _systemConfigRepository.GetByKeyAsync("AutoApproved");
            autoApprovedConfig.Value = input.isAutoApproved;
            await _systemConfigRepository.UpdateAsync(autoApprovedConfig);
            return true;
        }
    }
}
