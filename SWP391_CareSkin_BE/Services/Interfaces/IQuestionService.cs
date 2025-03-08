using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuestionService
    {
        List<QuestionResponseDTO> GetAllQuestions();
        QuestionResponseDTO GetQuestionById(int questionId);
        void CreateQuestion(CreateQuestionRequestDTO dto);
        void UpdateQuestion(UpdateQuestionRequestDTO dto);
        void DeleteQuestion(int questionId);
    }

}
