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
        public async Task<IActionResult> GetQuestionsByQuiz(int quizId)
        {
            try
            {
                var questions = await _questionService.GetQuestionsByQuizAsync(quizId);
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

        [HttpGet("quizzes/{questionId}")]
        public async Task<IActionResult> GetQAByQuiz(int questionId, bool includeAnswers)
        {
            try
            {
                var questions = await _questionService.GetQAByQuizAsync(questionId, includeAnswers);
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

        [HttpGet("questions/{questionId}")]
        public async Task<IActionResult> GetQuestionByIdAsync(int questionId)
        {
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }


        // POST: api/Q_A/quizzes/{quizId}/questions
        [HttpPost("quizzes/{quizId}/questions")]
        public async Task<IActionResult> CreateQuestion(int quizId, [FromBody] CreateQuestionDTO createQuestionDTO)
        {
            try
            {
                var createdQuestion = await _questionService.CreateQuestionAsync(quizId, createQuestionDTO);
                return Created($"/api/Q_A/questions/{createdQuestion.QuestionsId}", createdQuestion);
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

        [HttpGet("answers/{answerId}")]
        public async Task<IActionResult> GetAnswerByIdAsync(int answerId)
        {
            var answer = await _answerService.GetAnswerByIdAsync(answerId);
            if (answer == null)
            {
                return NotFound();
            }
            return Ok(answer);
        }


        // POST: api/Q_A/questions/{questionId}/answers
        [HttpPost("questions/{questionId}/answers")]
        public async Task<IActionResult> CreateAnswer(int questionId, [FromBody] CreateAnswerDTO createAnswerDTO)
        {
            try
            {
                var createdAnswer = await _answerService.CreateAnswerAsync(questionId, createAnswerDTO);
                return Created($"/api/Q_A/answers/{createdAnswer.AnswerId}", createdAnswer);
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
