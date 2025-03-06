using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int id);
        Task<Question> CreateQuestionAsync(QuestionDTO dto);
        Task<Question> UpdateQuestionAsync(int id, QuestionDTO dto);
        Task<bool> DeleteQuestionAsync(int id);
    }
}
