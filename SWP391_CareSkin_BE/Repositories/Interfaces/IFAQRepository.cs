using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IFAQRepository
    {
        Task<List<FAQ>> GetAllFAQsAsync();
        Task<FAQ?> GetFAQByIdAsync(int faqId);
        Task AddFAQAsync(FAQ faq);
        Task DeleteFAQAsync(FAQ faq);
        Task UpdateFAQAsync(FAQ faq);
    }
}
