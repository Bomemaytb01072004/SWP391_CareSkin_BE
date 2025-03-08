using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Quiz;
using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [ApiController]
    [Route("api/quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public ActionResult<List<QuizResponseDTO>> GetAllQuizzes() => Ok(_quizService.GetAllQuizzes());

        [HttpGet("{quizId}")]
        public ActionResult<QuizResponseDTO> GetQuizById(int quizId) => Ok(_quizService.GetQuizById(quizId));

        [HttpPost]
        public IActionResult CreateQuiz([FromBody] CreateQuizRequestDTO dto)
        {
            _quizService.CreateQuiz(dto);
            return Ok("Quiz created successfully.");
        }

        [HttpPut]
        public IActionResult UpdateQuiz([FromBody] UpdateQuizRequestDTO dto)
        {
            _quizService.UpdateQuiz(dto);
            return Ok("Quiz updated successfully.");
        }

        [HttpDelete("{quizId}")]
        public IActionResult DeleteQuiz(int quizId)
        {
            _quizService.DeleteQuiz(quizId);
            return Ok("Quiz deleted successfully.");
        }
    }


}

