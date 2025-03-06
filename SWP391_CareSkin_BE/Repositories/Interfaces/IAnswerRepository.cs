using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAllAsync();
        Task<Answer> GetByIdAsync(int id);
        Task<Answer> AddAsync(Answer answer);
        Task<Answer> UpdateAsync(Answer answer);
        Task<bool> DeleteAsync(int id);
    }
}
