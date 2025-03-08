using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Answer;
using SWP391_CareSkin_BE.DTOs.Responses.Answer;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class AnswerMapper
    {
        public static Answer ToEntity(CreateAnswerRequestDTO dto)
        {
            return new Answer
            {
                QuestionId = dto.QuestionId,
                AnswersContext = dto.AnswersContext,
                PointForSkinType = dto.PointForSkinType
            };
        }

        public static Answer ToEntity(UpdateAnswerRequestDTO dto, Answer answer)
        {
            answer.AnswersContext = dto.AnswersContext;
            answer.PointForSkinType = dto.PointForSkinType;
            return answer;
        }

        public static AnswerResponseDTO ToDTO(Answer answer)
        {
            return new AnswerResponseDTO
            {
                AnswerId = answer.AnswerId,
                QuestionId = answer.QuestionId,
                AnswersContext = answer.AnswersContext,
                PointForSkinType = answer.PointForSkinType
            };
        }
    }


}
