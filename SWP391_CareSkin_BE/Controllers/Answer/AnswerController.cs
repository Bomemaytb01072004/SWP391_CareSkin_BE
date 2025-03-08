using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnswers()
        {
            var answers = await _answerService.GetAllAnswersAsync();
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerById(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null) return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerDTO answerDto)
        {
            var createdAnswer = await _answerService.CreateAnswerAsync(answerDto);
            return CreatedAtAction(nameof(GetAnswerById), new { id = createdAnswer.AnswerId }, createdAnswer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var result = await _answerService.DeleteAnswerAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
