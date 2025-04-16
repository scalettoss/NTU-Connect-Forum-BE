using ForumBE.DTOs.ReportStatus;
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
    public class ReportStatusController : ControllerBase
    {
        private readonly IReportStatusService _reportStatusService;

        public ReportStatusController(IReportStatusService reportStatusService)
        {
            _reportStatusService = reportStatusService;
        }

        [HttpGet]
        public async Task<ResponseBase> GetAllReportStatus()
        {
            var reportsStatus = await _reportStatusService.GetAllReportStatusesAsync();
            return ResponseBase.Success(reportsStatus);
        }

        [HttpGet("{id}")]
        public async Task<ResponseBase> GetReportStatusById(int id)
        {
            var reportStatus = await _reportStatusService.GetReportStatusByIdAsync(id);
            return ResponseBase.Success(reportStatus);
        }

        [HttpPost]
        public async Task<ResponseBase> CreateReportStatus([FromBody] ReportStatusCreateRequestDto input)
        {
            var isCreated = await _reportStatusService.CreateReportStatusAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created report status failed.");
            }
            return ResponseBase.Success("Created report status successfully");
        }

        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateReportStatus(int id, [FromBody] ReportStatusUpdateRequestDto input)
        {
            var isUpdated = await _reportStatusService.UpdateReportStatusAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update report status failed.");
            }
            return ResponseBase.Success("Update report status successfully");
        }
    }
}
