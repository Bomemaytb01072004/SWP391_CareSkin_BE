using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetByQuizIdAsync(int quizId);
        Task<Question> GetByIdAsync(int questionId);
        Task<Question> CreateAsync(Question question);
        Task<Question> UpdateAsync(Question question);
        Task DeleteAsync(int questionId);
        Task<bool> ExistsAsync(int questionId);
    }
}
