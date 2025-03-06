using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Implementations;

[Route("api/history")]
[ApiController]
public class HistoryController : ControllerBase
{
    private readonly HistoryService _historyService;

    public HistoryController(HistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpPost]
    public async Task<ActionResult<History>> SaveHistory(HistoryDTO dto)
    {
        var history = await _historyService.SaveHistoryAsync(dto);
        return Ok(history);
    }
}
