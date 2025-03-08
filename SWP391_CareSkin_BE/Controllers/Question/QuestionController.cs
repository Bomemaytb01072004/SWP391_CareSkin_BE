using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public ActionResult<List<QuestionResponseDTO>> GetAllQuestions() => Ok(_questionService.GetAllQuestions());

        [HttpGet("{questionId}")]
        public ActionResult<QuestionResponseDTO> GetQuestionById(int questionId) => Ok(_questionService.GetQuestionById(questionId));

        [HttpPost]
        public IActionResult CreateQuestion([FromBody] CreateQuestionRequestDTO dto)
        {
            _questionService.CreateQuestion(dto);
            return Ok("Question created successfully.");
        }

        [HttpPut]
        public IActionResult UpdateQuestion([FromBody] UpdateQuestionRequestDTO dto)
        {
            _questionService.UpdateQuestion(dto);
            return Ok("Question updated successfully.");
        }

        [HttpDelete("{questionId}")]
        public IActionResult DeleteQuestion(int questionId)
        {
            _questionService.DeleteQuestion(questionId);
            return Ok("Question deleted successfully.");
        }
    }

}
