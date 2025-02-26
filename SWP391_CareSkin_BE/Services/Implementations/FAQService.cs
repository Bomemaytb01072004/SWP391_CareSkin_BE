using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;

        public FAQService(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<List<ShowFAQDTO>> GetAllFAQsAsync()
        {
            var faqs = await _faqRepository.GetAllFAQsAsync();
            return faqs.Select(f => new ShowFAQDTO
            {
                FAQId = f.FAQId,
                Question = f.Question,
                Answer = f.Answer,
            }).ToList();

        }
    }
}

