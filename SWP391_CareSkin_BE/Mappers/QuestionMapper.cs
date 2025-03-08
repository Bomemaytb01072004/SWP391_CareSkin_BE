using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class QuestionMapper
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

        public static Question ToEntity(QuestionDTO questionDTO)
        {
            return new Question
            {
                QuestionsId = questionDTO.QuestionsId,
                QuizId = questionDTO.QuizId,
                QuestionContext = questionDTO.QuestionContext
            };
        }
    }

}
