using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class AnswerMapper
    {
        public static AnswerDTO ToDTO(Answer answer)
        {
            return new AnswerDTO
            {
                AnswerId = answer.AnswerId,
                QuestionId = answer.QuestionId,
                AnswersContext = answer.AnswersContext,
                PointForSkinType = answer.PointForSkinType
            };
        }

        public static Answer ToEntity(AnswerDTO answerDTO)
        {
            return new Answer
            {
                AnswerId = answerDTO.AnswerId,
                QuestionId = answerDTO.QuestionId,
                AnswersContext = answerDTO.AnswersContext,
                PointForSkinType = answerDTO.PointForSkinType
            };
        }
    }

}
