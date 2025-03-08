using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
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

        [HttpGet]
        public async Task<IActionResult> GetAllHistories()
        {
            var histories = await _historyService.GetAllHistoriesAsync();
            return Ok(histories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistoryById(int id)
        {
            var history = await _historyService.GetHistoryByIdAsync(id);
            if (history == null) return NotFound();
            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHistory([FromBody] HistoryDTO historyDto)
        {
            var createdHistory = await _historyService.CreateHistoryAsync(historyDto);
            return CreatedAtAction(nameof(GetHistoryById), new { id = createdHistory.HistoryId }, createdHistory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            var result = await _historyService.DeleteHistoryAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
