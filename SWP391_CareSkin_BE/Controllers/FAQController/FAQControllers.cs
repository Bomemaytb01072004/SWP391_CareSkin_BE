using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers.FAQController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQControllers : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQControllers(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFAQs()
        {
            var faqs = await _faqService.GetAllFAQsAsync();
            return Ok(faqs);
        }
    }
}
