using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Quiz;
using SWP391_CareSkin_BE.DTOs.Responses.Quiz;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IQuizService
    {
        List<QuizResponseDTO> GetAllQuizzes();
        QuizResponseDTO GetQuizById(int quizId);
        void CreateQuiz(CreateQuizRequestDTO dto);
        void UpdateQuiz(UpdateQuizRequestDTO dto);
        void DeleteQuiz(int quizId);
    }

}
