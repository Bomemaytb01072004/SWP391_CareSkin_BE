using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task<Quiz> GetByIdAsync(int quizId);
        Task<Quiz> CreateAsync(Quiz quiz);
        Task<Quiz> UpdateAsync(Quiz quiz);
        Task DeleteAsync(int quizId);
        Task<bool> ExistsAsync(int quizId);
    }
}
