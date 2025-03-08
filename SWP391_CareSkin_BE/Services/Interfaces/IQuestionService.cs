using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDTO>> GetAllQuestionsAsync();
        Task<QuestionDTO> GetQuestionByIdAsync(int id);
        Task<QuestionDTO> CreateQuestionAsync(QuestionDTO questionDto);
        Task<QuestionDTO> UpdateQuestionAsync( QuestionDTO questionDto);
        Task<bool> DeleteQuestionAsync(int id);
    }
}
