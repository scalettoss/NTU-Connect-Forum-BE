using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        private IStatisticsService _statisticsService;
        public StatisticController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpGet("count")]
        public async Task<ResponseBase> CountAllModule()
        {
            var num = await _statisticsService.GetStatisticsAsync();
            return ResponseBase.Success(num);
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpGet("activity")]
        public async Task<ResponseBase> GetLatestActivity()
        {
            var activities = await _statisticsService.GetLatestActivityAsync();
            return ResponseBase.Success(activities);
        }
    }
}
