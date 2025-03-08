using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<IEnumerable<AnswerDTO>> GetAllAsync();
        Task<AnswerDTO> GetByIdAsync(int id);
        Task<AnswerDTO> AddAsync(AnswerDTO answerDTO);
        Task<AnswerDTO> UpdateAsync(AnswerDTO answerDTO);
        Task<bool> DeleteAsync(int id);
    }
}
