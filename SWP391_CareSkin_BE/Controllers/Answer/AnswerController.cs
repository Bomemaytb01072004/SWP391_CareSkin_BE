using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Answer;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/answer")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public IActionResult GetAllAnswers()
        {
            var answers = _answerService.GetAllAnswers();
            return Ok(answers);
        }

        [HttpGet("{id}")]
        public IActionResult GetAnswerById(int id)
        {
            var answer = _answerService.GetAnswerById(id);
            if (answer == null)
                return NotFound("Answer not found");
            return Ok(answer);
        }

        [HttpPost]
        public IActionResult CreateAnswer([FromBody] CreateAnswerRequestDTO dto)
        {
            _answerService.CreateAnswer(dto);
            return Ok("Answer created successfully");
        }

        [HttpPut]
        public IActionResult UpdateAnswer([FromBody] UpdateAnswerRequestDTO dto)
        {
            _answerService.UpdateAnswer(dto);
            return Ok("Answer updated successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(int id)
        {
            _answerService.DeleteAnswer(id);
            return Ok("Answer deleted successfully");
        }
    }
}
