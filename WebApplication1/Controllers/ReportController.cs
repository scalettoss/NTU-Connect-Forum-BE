using ForumBE.DTOs.Reports;
using ForumBE.Response;
using ForumBE.Services.Implementations;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ResponseBase> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return ResponseBase.Success(reports);
        }

        [HttpGet("{id}")]
        public async Task<ResponseBase> GetReportById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            return ResponseBase.Success(report);
        }

        [HttpPost]
        public async Task<ResponseBase> CreateReport([FromBody] ReportCreateRequestDto input)
        {
            var isCreated = await _reportService.CreateReportAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created report failed.");
            }
            return ResponseBase.Success("Created report successfully");


        }

        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateReport(int id, [FromBody] ReportUpdateRequestDto input)
        {
            var isUpdated = await _reportService.UpdateReportAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update report failed.");
            }
            return ResponseBase.Success("Update report successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteReport(int id)
        {
            var isDeleted = await _reportService.DeleteReportAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete report failed.");
            }
            return ResponseBase.Success("Delete report successfully");
        }
    }
}
