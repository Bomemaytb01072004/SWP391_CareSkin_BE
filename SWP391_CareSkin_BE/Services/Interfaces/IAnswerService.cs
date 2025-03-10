using SWP391_CareSkin_BE.DTOS.Requests.Answer;
using SWP391_CareSkin_BE.DTOS.Responses.Answer;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<List<AnswerDTO>> GetAnswersByQuestionAsync(int questionId);
        Task<AnswerDTO> GetAnswerByIdAsync(int answerId);
        Task<AnswerDTO> CreateAnswerAsync(int questionId, CreateAnswerDTO createAnswerDTO);
        Task<AnswerDTO> UpdateAnswerAsync(int answerId, UpdateAnswerDTO updateAnswerDTO);
        Task DeleteAnswerAsync(int answerId);
    }
}
