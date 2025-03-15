using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task<IEnumerable<Quiz>> GetActiveAsync();
        Task<IEnumerable<Quiz>> GetInactiveAsync();
        Task<Quiz> GetByIdAsync(int quizId);
        Task<Quiz> CreateAsync(Quiz quiz);
        Task<Quiz> UpdateAsync(Quiz quiz);
        Task<bool> ExistsAsync(int quizId);
        Task<Quiz> GetByTitleAsync(string title);
    }
}
