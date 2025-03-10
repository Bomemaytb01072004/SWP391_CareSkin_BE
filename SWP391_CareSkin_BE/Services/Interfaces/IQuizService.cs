using SWP391_CareSkin_BE.DTOS.Requests.Quiz;
using SWP391_CareSkin_BE.DTOS.Responses.Quiz;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizDTO>> GetAllQuizzesAsync();
        Task<QuizDTO> GetQuizByIdAsync(int quizId);
        Task<QuizDTO> CreateQuizAsync(CreateQuizDTO createQuizDTO);
        Task<QuizDTO> UpdateQuizAsync(int quizId, UpdateQuizDTO updateQuizDTO);
        Task DeleteQuizAsync(int quizId);
    }
}
