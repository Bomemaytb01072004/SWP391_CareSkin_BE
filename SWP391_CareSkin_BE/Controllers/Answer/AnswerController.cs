using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Services.Implementations;

namespace SWP391_CareSkin_BE.Controllers.Answer
{
    [Route("api/answers")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly AnswerService _answerService;

        public AnswerController(AnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Answer>>> GetAllAnswers()
        {
            return Ok(await _answerService.GetAllAnswersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswerById(int id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null) return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<Answer>> CreateAnswer(AnswerDTO dto)
        {
            var createdAnswer = await _answerService.CreateAnswerAsync(dto);
            return CreatedAtAction(nameof(GetAnswerById), new { id = createdAnswer.AnswerId }, createdAnswer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, AnswerDTO dto)
        {
            var updatedAnswer = await _answerService.UpdateAnswerAsync(id, dto);
            if (updatedAnswer == null) return NotFound();
            return Ok(updatedAnswer);
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
