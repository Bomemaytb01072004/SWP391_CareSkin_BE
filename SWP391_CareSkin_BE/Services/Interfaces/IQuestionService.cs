using SWP391_CareSkin_BE.DTOS.Requests.Question;
using SWP391_CareSkin_BE.DTOS.Responses.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionDTO>> GetQuestionsByQuizAsync(int quizId);
        Task<QuestionDTO> GetQuestionByIdAsync(int questionId);
        Task<QuestionDetailsDTO> GetQAByQuizAsync(int questionId, bool includeAnswers);
        Task<QuestionDTO> CreateQuestionAsync(int quizId, CreateQuestionDTO createQuestionDTO);
        Task<QuestionDTO> UpdateQuestionAsync(int questionId, UpdateQuestionDTO updateQuestionDTO);
        Task DeleteQuestionAsync(int questionId);
    }
}
