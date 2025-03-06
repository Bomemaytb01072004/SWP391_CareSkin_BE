using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/questions")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly QuestionService _questionService;

    public QuestionController(QuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Question>>> GetAllQuestions()
    {
        return Ok(await _questionService.GetAllQuestionsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetQuestionById(int id)
    {
        var question = await _questionService.GetQuestionByIdAsync(id);
        if (question == null) return NotFound();
        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<Question>> CreateQuestion(QuestionDTO dto)
    {
        var createdQuestion = await _questionService.CreateQuestionAsync(dto);
        return CreatedAtAction(nameof(GetQuestionById), new { id = createdQuestion.QuestionsId }, createdQuestion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion(int id, QuestionDTO dto)
    {
        var updatedQuestion = await _questionService.UpdateQuestionAsync(id, dto);
        if (updatedQuestion == null) return NotFound();
        return Ok(updatedQuestion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var result = await _questionService.DeleteQuestionAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
