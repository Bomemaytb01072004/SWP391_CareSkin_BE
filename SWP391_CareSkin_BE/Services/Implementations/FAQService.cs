using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOs.Requests.FAQ;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using SWP391_CareSkin_BE.Mappers;

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
            return FAQMapper.ToShowFAQDTOList(faqs);
        }

        public async Task<ShowFAQDTO?> GetFAQByIdAsync(int faqId)
        {
            var faq = await _faqRepository.GetFAQByIdAsync(faqId);
            return faq != null ? FAQMapper.ToShowFAQDTO(faq) : null;
        }

        public async Task AddFAQAsync(CreateFAQDTO dto)
        {
            var faq = FAQMapper.ToFAQ(dto);
            await _faqRepository.AddFAQAsync(faq);
        }

        public async Task<bool> UpdateFAQAsync(int faqId, UpdateFAQDTO dto)
        {
            var faq = await _faqRepository.GetFAQByIdAsync(faqId);
            if (faq == null) return false;

            FAQMapper.UpdateFAQ(faq, dto);
            await _faqRepository.UpdateFAQAsync(faq);
            return true;
        }

        public async Task<bool> DeleteFAQAsync(int faqId)
        {
            var faq = await _faqRepository.GetFAQByIdAsync(faqId);
            if (faq == null) return false;

            await _faqRepository.DeleteFAQAsync(faq);
            return true;
        }
    }
}
