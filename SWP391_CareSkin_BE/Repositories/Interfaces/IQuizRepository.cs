using SWP391_CareSkin_BE.DTOs.Requests.Quiz;
using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        List<Quiz> GetAllQuizzes();
        Quiz GetQuizById(int quizId);
        void AddQuiz(Quiz quiz);
        void UpdateQuiz(Quiz quiz);
        void DeleteQuiz(int quizId);
        void SaveChanges();
    }


}
