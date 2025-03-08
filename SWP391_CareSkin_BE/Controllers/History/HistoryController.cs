using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.History;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
        [Route("api/history")]
        [ApiController]
        public class HistoryController : ControllerBase
        {
            private readonly IHistoryService _historyService;

            public HistoryController(IHistoryService historyService)
            {
                _historyService = historyService;
            }

            [HttpGet]
            public IActionResult GetAllHistories()
            {
                var histories = _historyService.GetAllHistories();
                return Ok(histories);
            }

            [HttpGet("{id}")]
            public IActionResult GetHistoryById(int id)
            {
                var history = _historyService.GetHistoryById(id);
                if (history == null)
                    return NotFound("History not found");
                return Ok(history);
            }

            [HttpPost]
            public IActionResult CreateHistory([FromBody] CreateHistoryRequestDTO dto)
            {
                _historyService.CreateHistory(dto);
                return Ok("History created successfully");
            }

            [HttpPut]
            public IActionResult UpdateHistory([FromBody] UpdateHistoryRequestDTO dto)
            {
                _historyService.UpdateHistory(dto);
                return Ok("History updated successfully");
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteHistory(int id)
            {
                _historyService.DeleteHistory(id);
                return Ok("History deleted successfully");
            }
        }  

}
