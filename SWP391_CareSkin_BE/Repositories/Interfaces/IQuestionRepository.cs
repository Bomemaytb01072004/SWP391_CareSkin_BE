using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<QuestionDTO>> GetAllAsync();
        Task<QuestionDTO> GetByIdAsync(int id);
        Task<QuestionDTO> AddAsync(QuestionDTO questionDTO);
        Task<QuestionDTO> UpdateAsync(QuestionDTO questionDTO);
        Task<bool> DeleteAsync(int id);
    }
}
