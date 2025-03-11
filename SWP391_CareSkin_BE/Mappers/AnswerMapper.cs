using SWP391_CareSkin_BE.DTOS.Requests.Answer;
using SWP391_CareSkin_BE.DTOS.Responses.Answer;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class AnswerMapper
    {
        public static AnswerDTO ToDTO(Answer answer)
        {
            return new AnswerDTO
            {
                QuestionId = answer.QuestionId,
                AnswerId = answer.AnswerId,
                AnswersText = answer.AnswersText,
                Score = answer.Score
            };
        }

        public static List<AnswerDTO> ToDTOList(IEnumerable<Answer> answers)
        {
            return answers.Select(ToDTO).ToList();
        }

        public static Answer ToEntity(int questionId, CreateAnswerDTO createAnswerDTO)
        {
            return new Answer
            {
                QuestionId = questionId,
                AnswersText = createAnswerDTO.AnswersText,
                Score = createAnswerDTO.Score
            };
        }

        public static void UpdateEntity(Answer answer, UpdateAnswerDTO updateAnswerDTO)
        {
            answer.AnswersText = updateAnswerDTO.AnswersText;
            answer.Score = updateAnswerDTO.Score;
        }
    }
}
