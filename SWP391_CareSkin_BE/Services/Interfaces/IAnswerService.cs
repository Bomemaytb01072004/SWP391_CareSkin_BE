using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<List<Answer>> GetAllAnswersAsync();
        Task<Answer> GetAnswerByIdAsync(int id);
        Task<Answer> CreateAnswerAsync(AnswerDTO dto);
        Task<Answer> UpdateAnswerAsync(int id, AnswerDTO dto);
        Task<bool> DeleteAnswerAsync(int id);

    }
}
