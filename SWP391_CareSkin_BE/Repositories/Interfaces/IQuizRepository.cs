using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<QuizDTO>> GetAllAsync();
        Task<QuizDTO> GetByIdAsync(int id);
        Task<QuizDTO> AddAsync(QuizDTO quizDTO);
        Task<QuizDTO> UpdateAsync(QuizDTO quizDTO);
        Task<bool> DeleteAsync(int id);
    }
}
