using SWP391_CareSkin_BE.DTOs;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<IEnumerable<AnswerDTO>> GetAllAnswersAsync();
        Task<AnswerDTO> GetAnswerByIdAsync(int id);
        Task<AnswerDTO> CreateAnswerAsync(AnswerDTO answerDto);
        Task<AnswerDTO> UpdateAnswerAsync(int id, AnswerDTO answerDto);
        Task<bool> DeleteAnswerAsync(int id);
    }
}
