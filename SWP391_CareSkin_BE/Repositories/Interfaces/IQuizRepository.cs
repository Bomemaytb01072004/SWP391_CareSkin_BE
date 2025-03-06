using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetAllAsync();
        Task<Quiz> GetByIdAsync(int id);
        Task<Quiz> AddAsync(Quiz quiz);
        Task<Quiz> UpdateAsync(Quiz quiz);
        Task<bool> DeleteAsync(int id);


    }
}
