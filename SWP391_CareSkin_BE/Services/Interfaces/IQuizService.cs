using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.DTOS.Requests.Quiz;
using SWP391_CareSkin_BE.DTOS.Responses.Quiz;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDTO>> GetAllQuizzesAsync();
        Task<List<QuizDTO>> GetActiveQuizzesAsync();
        Task<List<QuizDTO>> GetInactiveQuizzesAsync();
        Task<QuizDetailsDTO> GetQuizByIdAsync(int quizId);
        Task<QuizDTO> CreateQuizAsync(CreateQuizDTO createQuizDTO);
        Task<QuizDTO> UpdateQuizAsync(int quizId, UpdateQuizDTO updateQuizDTO);
        Task DeleteQuizAsync(int quizId);
    }
}
