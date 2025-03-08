using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDTO>> GetAllQuizzesAsync();
        Task<QuizDTO> GetQuizByIdAsync(int id);
        Task<QuizDTO> CreateQuizAsync(QuizDTO quizDto);
        Task<QuizDTO> UpdateQuizAsync(int id, QuizDTO quizDto);
        Task<bool> DeleteQuizAsync(int id);
    }
}
