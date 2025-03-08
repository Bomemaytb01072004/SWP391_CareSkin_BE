using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizDTO quizDto)
        {
            var createdQuiz = await _quizService.CreateQuizAsync(quizDto);
            return CreatedAtAction(nameof(GetQuizById), new { id = createdQuiz.QuizId }, createdQuiz);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var result = await _quizService.DeleteQuizAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
