using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Quiz;
using SWP391_CareSkin_BE.DTOS.Responses.Quiz;
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

        // GET: api/Quiz
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDTO>>> GetAllQuizzes()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return Ok(quizzes);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<QuizDTO>>> GetActiveQuizzes()
        {
            var quizzes = await _quizService.GetActiveQuizzesAsync();
            return Ok(quizzes);
        }

        [HttpGet("inactive")]
        public async Task<ActionResult<List<QuizDTO>>> GetInactiveQuizzes()
        {
            var quizzes = await _quizService.GetInactiveQuizzesAsync();
            return Ok(quizzes);
        }

        // GET: api/Quiz/{quizId}
        [HttpGet("{quizId}")]
        public async Task<IActionResult> GetQuizById(int quizId)
        {
            try
            {
                var quiz = await _quizService.GetQuizByIdAsync(quizId);
                return Ok(quiz);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Quiz
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDTO createQuizDTO)
        {
            try
            {
                var createdQuiz = await _quizService.CreateQuizAsync(createQuizDTO);
                return CreatedAtAction(nameof(GetQuizById), new { quizId = createdQuiz.QuizId }, createdQuiz);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Quiz/{quizId}
        [HttpPut("{quizId}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] UpdateQuizDTO updateQuizDTO)
        {
            try
            {
                var updatedQuiz = await _quizService.UpdateQuizAsync(quizId, updateQuizDTO);
                return Ok(updatedQuiz);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Quiz/{quizId}
        [HttpDelete("{quizId}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            try
            {
                await _quizService.DeleteQuizAsync(quizId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
