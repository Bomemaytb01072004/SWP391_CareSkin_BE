using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.Jobs;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly PromotionUpdaterJob _promotionUpdaterJob;

        public HangfireController(PromotionUpdaterJob promotionUpdaterJob)
        {
            _promotionUpdaterJob = promotionUpdaterJob;
        }

        [HttpPost("trigger-promotion-update")]
        [Authorize(Roles = "Admin")]
        public IActionResult TriggerPromotionUpdate()
        {
            // Trigger the job immediately
            BackgroundJob.Enqueue(() => _promotionUpdaterJob.UpdatePromotionStatusesAsync());
            return Ok(new { message = "Promotion update job has been triggered." });
        }

        [HttpGet("job-status")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetJobStatus()
        {
            // This endpoint is mainly for checking if Hangfire is properly configured
            // Actual job status would be visible in the Hangfire dashboard
            return Ok(new { message = "Hangfire is running. Check the dashboard for job status." });
        }
    }
}
