using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class QuestionMapper
    {
        public static Question ToEntity(CreateQuestionRequestDTO dto)
        {
            return new Question
            {
                QuizId = dto.QuizId,
                QuestionContext = dto.QuestionContext
            };
        }

        public static Question ToEntity(UpdateQuestionRequestDTO dto, Question question)
        {
            question.QuestionContext = dto.QuestionContext;
            return question;
        }

        public static QuestionResponseDTO ToDTO(Question question)
        {
            return new QuestionResponseDTO
            {
                QuestionsId = question.QuestionsId,
                QuizId = question.QuizId,
                QuestionContext = question.QuestionContext
            };
        }
    }



}
