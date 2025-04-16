using ForumBE.DTOs.Warnings;
using ForumBE.Response;
using ForumBE.Services.Implementations;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarningController : ControllerBase
    {
        private readonly IWarningService _warningService;
        public WarningController(IWarningService warningService)
        {
            _warningService = warningService;
        }

        [HttpGet]
        public async Task<ResponseBase> GetAllWarnings()
        {
            var warnings = await _warningService.GetAllWarningsAsync();
            return ResponseBase.Success(warnings);
        }
        [HttpGet("count/{userId}")]
        public async Task<ResponseBase> GetWarningsCountByUserId(int userId)
        {
            var count = await _warningService.GetWarningsCountByUserId(userId);
            return ResponseBase.Success(count);
        }
        [HttpPost]
        public async Task<ResponseBase> CreateWarning([FromBody] WarningCreateRequestDto input)
        {
            var isCreated = await _warningService.CreateWarningAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Failed to create warning");
            }
            return ResponseBase.Success("Warning created successfully");
        }
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateWarning(int id, [FromBody] WarningUpdateRequestDto input)
        {
            var isUpdated = await _warningService.UpdateWarningAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Failed to update warning");
            }
            return ResponseBase.Success("Warning updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteWarning(int id)
        {
            var result = await _warningService.DeleteWarningAsync(id);
            if (!result)
            {
                return ResponseBase.Fail("Failed to delete warning");
            }
            return ResponseBase.Success("Warning deleted successfully");
        }


    }
}
