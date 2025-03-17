using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.History;
using SWP391_CareSkin_BE.DTOS.Responses.History;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Collections.Generic;

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


        [HttpPost("attempt/{userQuizAttemptId}")]
        public async Task<ActionResult<List<HistoryDTO>>> CreateHistories(int userQuizAttemptId, [FromBody] List<CreateHistoryDTO> createHistoryDTOs)
        {
            try
            {
                var histories = await _historyService.CreateHistoriesAsync(userQuizAttemptId, createHistoryDTOs);
                return Ok(histories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("attempt/{userQuizAttemptId}/answer")]
        public async Task<ActionResult<HistoryDTO>> CreateOrUpdateHistory(int userQuizAttemptId, [FromBody] CreateHistoryDTO historyDTO)
        {
            try
            {
                var history = await _historyService.CreateOrUpdateHistoryAsync(userQuizAttemptId, historyDTO);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("attempt/{userQuizAttemptId}")]
        public async Task<ActionResult<List<HistoryDTO>>> GetHistoriesByAttemptId(int userQuizAttemptId, [FromQuery] bool includeDetails = false)
        {
            try
            {
                var histories = await _historyService.GetHistoriesByAttemptIdAsync(userQuizAttemptId, includeDetails);
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
