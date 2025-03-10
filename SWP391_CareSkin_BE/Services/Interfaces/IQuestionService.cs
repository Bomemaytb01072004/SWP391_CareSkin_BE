using SWP391_CareSkin_BE.DTOS.Requests.Question;
using SWP391_CareSkin_BE.DTOS.Responses.Question;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionDTO>> GetQuestionsByQuizAsync(int quizId);
        Task<QuestionDTO> GetQuestionByIdAsync(int questionId, bool includeAnswers = false);
        Task<QuestionDTO> CreateQuestionAsync(int quizId, CreateQuestionDTO createQuestionDTO);
        Task<QuestionDTO> UpdateQuestionAsync(int questionId, UpdateQuestionDTO updateQuestionDTO);
        Task DeleteQuestionAsync(int questionId);
    }
}
