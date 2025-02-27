using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.Services.Interfaces;
using SWP391_CareSkin_BE.DTOs.Requests.FAQ;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllFAQs()
        {
            var faqs = await _faqService.GetAllFAQsAsync();
            return Ok(faqs);
        }

        [HttpGet("GetFAQById/{id}")]
        public async Task<IActionResult> GetFAQById(int id)
        {
            var faq = await _faqService.GetFAQByIdAsync(id);
            if (faq == null) return NotFound("FAQ not found");
            return Ok(faq);
        }

        [HttpPost("AddFAQ")]
        public async Task<IActionResult> CreateFAQ([FromBody] CreateFAQDTO dto)
        {
            await _faqService.AddFAQAsync(dto);
            return CreatedAtAction(nameof(GetFAQById), new { id = dto.Question }, dto);
        }

        [HttpPut("UpdateFAQ/{id}")]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] UpdateFAQDTO dto)
        {
            var updated = await _faqService.UpdateFAQAsync(id, dto);
            if (!updated) return NotFound("FAQ not found");
            return Ok(new { message = "FAQ updated successfully" });
        }

        [HttpDelete("DeleteFAQ/{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var deleted = await _faqService.DeleteFAQAsync(id);
            if (!deleted) return NotFound("FAQ not found");
            return Ok(new { message = "FAQ deleted successfully" });
        }
    }
}
