using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.History;
using SWP391_CareSkin_BE.DTOS.Responses.History;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpPost("attempt/{attemptId}")]
        public async Task<ActionResult<HistoryDTO>> CreateHistory(int attemptId, [FromBody] CreateHistoryDTO createHistoryDTO)
        {
            try
            {
                var history = await _historyService.CreateHistoryAsync(attemptId, createHistoryDTO);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("attempt/{attemptId}")]
        public async Task<ActionResult<List<HistoryDTO>>> GetHistoriesByAttemptId(int attemptId, [FromQuery] bool includeDetails = false)
        {
            try
            {
                var histories = await _historyService.GetHistoriesByAttemptIdAsync(attemptId, includeDetails);
                return Ok(histories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{historyId}")]
        public async Task<ActionResult<HistoryDTO>> GetHistoryById(int historyId, [FromQuery] bool includeDetails = false)
        {
            try
            {
                var history = await _historyService.GetHistoryByIdAsync(historyId, includeDetails);
                
                if (history == null)
                {
                    return NotFound($"History with ID {historyId} not found");
                }
                
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
