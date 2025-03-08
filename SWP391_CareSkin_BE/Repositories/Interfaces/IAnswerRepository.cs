using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Answer;
using SWP391_CareSkin_BE.DTOs.Requests.Quiz;
using SWP391_CareSkin_BE.DTOs.Responses.Answer;
using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        List<Answer> GetAllAnswers();
        Answer GetAnswerById(int answerId);
        void AddAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(int answerId);
        void SaveChanges();
    }

}
