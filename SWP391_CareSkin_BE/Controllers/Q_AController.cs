using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Q_AController : ControllerBase
    {
        //private readonly IQuestionService _questionService;
        //private readonly IAnswerService _answerService;

        //public Q_AController(IQuestionService questionService, IAnswerService answerService)
        //{
        //    _questionService = questionService;
        //    _answerService = answerService;
        //}

        //// GET: api/Q_A
        //[HttpGet("quizzes/{quizId}/questions")]
        //public async Task<IActionResult> GetQuestionsByQuiz(int quizId)
        //{
        //    var questions = await _questionService.GetQuestionsByQuizAsync(quizId);
        //    return Ok(questions);
        //}

        //// POST: /api/q_a/quizzes/{quizId}/questions
        //[HttpPost("quizzes/{quizId}/questions")]
        //public async Task<IActionResult> CreateQuestion(int quizId, [FromBody] QuestionDto questionDto)
        //{

        //}

        //// GET: /api/q_a/questions/{questionId}
        //[HttpGet("questions/{questionId}")]
        //public async Task<IActionResult> GetQuestion(int questionId)
        //{

        //}

        //// PUT: /api/q_a/questions/{questionId}
        //[HttpPut("questions/{questionId}")]
        //public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] QuestionDto questionDto)
        //{

        //}

        //// DELETE: /api/q_a/questions/{questionId}
        //[HttpDelete("questions/{questionId}")]
        //public async Task<IActionResult> DeleteQuestion(int questionId)
        //{

        //}

        //// --- Answer Methods --- //

        //// GET: /api/q_a/questions/{questionId}/answers
        //[HttpGet("questions/{questionId}/answers")]
        //public async Task<IActionResult> GetAnswersByQuestion(int questionId)
        //{

        //}

        //// POST: /api/q_a/questions/{questionId}/answers
        //[HttpPost("questions/{questionId}/answers")]
        //public async Task<IActionResult> CreateAnswer(int questionId, [FromBody] AnswerDto answerDto)
        //{

        //}

        //// PUT: /api/q_a/answers/{answerId}
        //[HttpPut("answers/{answerId}")]
        //public async Task<IActionResult> UpdateAnswer(int answerId, [FromBody] AnswerDto answerDto)
        //{

        //}

        //// DELETE: /api/q_a/answers/{answerId}
        //[HttpDelete("answers/{answerId}")]
        //public async Task<IActionResult> DeleteAnswer(int answerId)
        //{

        //}
    }
}

