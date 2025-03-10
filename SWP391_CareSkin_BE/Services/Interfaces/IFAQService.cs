using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Requests.FAQ;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IFAQService
    {
        Task<List<ShowFAQDTO>> GetAllFAQsAsync();

        Task<ShowFAQDTO?> GetFAQByIdAsync(int faqId);

        Task AddFAQAsync(CreateFAQDTO dto);

        Task<bool> UpdateFAQAsync(int faqId, UpdateFAQDTO dto);

        Task<bool> DeleteFAQAsync(int faqId);
    }
}
