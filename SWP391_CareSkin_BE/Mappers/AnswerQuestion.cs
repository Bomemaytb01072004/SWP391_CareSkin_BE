using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class AnswerQuestion
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

        public static Answer ToEntity(AnswerDTO dto)
        {
            return new Answer
            {
                AnswerId = dto.AnswerId,
                QuestionId = dto.QuestionId,
                AnswersContext = dto.AnswersContext,
                PointForSkinType = dto.PointForSkinType
            };
        }
    }
}
