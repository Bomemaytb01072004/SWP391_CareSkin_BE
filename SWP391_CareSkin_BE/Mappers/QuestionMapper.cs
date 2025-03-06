using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class QuestionMapper
    {
        public static QuestionDTO ToDTO(Question question)
        {
            return new QuestionDTO
            {
                QuestionsId = question.QuestionsId,
                QuizId = question.QuizId,
                QuestionContext = question.QuestionContext
            };
        }

        public static Question ToEntity(QuestionDTO dto)
        {
            return new Question
            {
                QuestionsId = dto.QuestionsId,
                QuizId = dto.QuizId,
                QuestionContext = dto.QuestionContext
            };
        }
    }
}
