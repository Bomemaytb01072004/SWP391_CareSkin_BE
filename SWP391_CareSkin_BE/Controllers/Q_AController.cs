using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests.Answer;
using SWP391_CareSkin_BE.DTOS.Requests.Question;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q_AController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;

        public Q_AController(IQuestionService questionService, IAnswerService answerService)
        {
            _questionService = questionService;
            _answerService = answerService;
        }

        // GET: api/Q_A/quizzes/{quizId}/questions
        [HttpGet("quizzes/{quizId}/questions")]
        public async Task<IActionResult> GetQuestionsByQuiz(int quizId, [FromQuery] bool includeAnswers = false)
        {
            try
            {
                var questions = await _questionService.GetQuestionsByQuizAsync(quizId, includeAnswers);
                return Ok(questions);
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

        // POST: api/Q_A/quizzes/{quizId}/questions
        [HttpPost("quizzes/{quizId}/questions")]
        public async Task<IActionResult> CreateQuestion(int quizId, [FromBody] CreateQuestionDTO createQuestionDTO)
        {
            try
            {
                var createdQuestion = await _questionService.CreateQuestionAsync(quizId, createQuestionDTO);
                return CreatedAtAction(nameof(GetQuestion), new { questionId = createdQuestion.QuestionsId }, createdQuestion);
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

        // GET: api/Q_A/questions/{questionId}
        [HttpGet("questions/{questionId}")]
        public async Task<IActionResult> GetQuestion(int questionId, [FromQuery] bool includeAnswers = false)
        {
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(questionId, includeAnswers);
                return Ok(question);
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

        // PUT: api/Q_A/questions/{questionId}
        [HttpPut("questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] UpdateQuestionDTO updateQuestionDTO)
        {
            try
            {
                var updatedQuestion = await _questionService.UpdateQuestionAsync(questionId, updateQuestionDTO);
                return Ok(updatedQuestion);
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

        // DELETE: api/Q_A/questions/{questionId}
        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            try
            {
                await _questionService.DeleteQuestionAsync(questionId);
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

        // --- Answer Methods --- //

        // GET: api/Q_A/questions/{questionId}/answers
        [HttpGet("questions/{questionId}/answers")]
        public async Task<IActionResult> GetAnswersByQuestion(int questionId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByQuestionAsync(questionId);
                return Ok(answers);
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

        // GET: api/Q_A/answers/{answerId}
        [HttpGet("answers/{answerId}")]
        public async Task<IActionResult> GetAnswer(int answerId)
        {
            try
            {
                var answer = await _answerService.GetAnswerByIdAsync(answerId);
                return Ok(answer);
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

        // POST: api/Q_A/questions/{questionId}/answers
        [HttpPost("questions/{questionId}/answers")]
        public async Task<IActionResult> CreateAnswer(int questionId, [FromBody] CreateAnswerDTO createAnswerDTO)
        {
            try
            {
                var createdAnswer = await _answerService.CreateAnswerAsync(questionId, createAnswerDTO);
                return CreatedAtAction(nameof(GetAnswer), new { answerId = createdAnswer.AnswerId }, createdAnswer);
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

        // PUT: api/Q_A/answers/{answerId}
        [HttpPut("answers/{answerId}")]
        public async Task<IActionResult> UpdateAnswer(int answerId, [FromBody] UpdateAnswerDTO updateAnswerDTO)
        {
            try
            {
                var updatedAnswer = await _answerService.UpdateAnswerAsync(answerId, updateAnswerDTO);
                return Ok(updatedAnswer);
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

        // DELETE: api/Q_A/answers/{answerId}
        [HttpDelete("answers/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(int answerId)
        {
            try
            {
                await _answerService.DeleteAnswerAsync(answerId);
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
