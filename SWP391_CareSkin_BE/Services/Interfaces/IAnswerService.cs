using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Answer;
using SWP391_CareSkin_BE.DTOs.Responses.Answer;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAnswerService
    {
        List<AnswerResponseDTO> GetAllAnswers();
        AnswerResponseDTO GetAnswerById(int answerId);
        void CreateAnswer(CreateAnswerRequestDTO dto);
        void UpdateAnswer(UpdateAnswerRequestDTO dto);
        void DeleteAnswer(int answerId);
    }

}
