using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Services.Implementations;

namespace SWP391_CareSkin_BE.Controllers.Quiz
{
    [Route("api/quizzes")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<ActionResult<List<QuizDTO>>> GetAllQuizzes()
        {
            return Ok(await _quizService.GetAllQuizzesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDTO>> GetQuizById(int id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        [HttpPost]
        public async Task<ActionResult<QuizDTO>> CreateQuiz(QuizDTO dto)
        {
            var createdQuiz = await _quizService.CreateQuizAsync(dto);
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
