using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.SystemConfigs;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.SystemConfigs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemConfigController : ControllerBase
    {
        private readonly ISystemConfigService _systemConfigService;
        public SystemConfigController(ISystemConfigService systemConfigService)
        {
            _systemConfigService = systemConfigService;
        }

        [HttpPost("set-auto-approved")]
        [AuthorizeRoles(ConstantString.Admin)]
        public async Task<ResponseBase> SetAutoApproved([FromBody] SystemConfigRequestDto request)
        {
            var isSet = await _systemConfigService.SetAutoApproved(request);
            if (!isSet)
            {
                return ResponseBase.Fail("Failed to set auto-approved configuration.");
            }
            return ResponseBase.Success("Success to set auto-approved configuration.");
        }

        [HttpGet("get-auto-approved")]
        [AuthorizeRoles(ConstantString.Admin)]
        public async Task<ResponseBase> GetAutoApproved()
        {
            var isAuto = await _systemConfigService.GetAutoApproved();
            return ResponseBase.Success(isAuto);
        }
    }
}
