using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetAllQuizzesAsync();
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<Quiz> CreateQuizAsync(QuizDTO dto);
        Task<Quiz> UpdateQuizAsync(int id, QuizDTO dto);
        Task<bool> DeleteQuizAsync(int id);

    }
}
