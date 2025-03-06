using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllAsync();
        Task<Question> AddAsync(Question question);
        Task<Question> UpdateAsync(Question question);
        Task<bool> DeleteAsync(int id);
    }
}
