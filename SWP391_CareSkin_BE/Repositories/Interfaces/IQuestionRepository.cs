using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        List<Question> GetAllQuestions();
        Question GetQuestionById(int questionId);
        void AddQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(int questionId);
        void SaveChanges();
    }

}
