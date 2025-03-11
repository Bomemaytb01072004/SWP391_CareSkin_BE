using SWP391_CareSkin_BE.DTOS.Requests.Question;
using SWP391_CareSkin_BE.DTOS.Responses.Question;
using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class QuestionMapper
    {
        // Basic DTO without answers
        public static QuestionDTO ToDTO(Question question)
        {
            return new QuestionDTO
            {
                QuestionsId = question.QuestionsId,
                QuizId = question.QuizId,
                QuestionText = question.QuestionText
            };
        }

        public static List<QuestionDTO> ToDTOList(IEnumerable<Question> questions)
        {
            return questions.Select(q => ToDTO(q)).ToList();
        }

        // Detailed DTO with answers
        public static QuestionDetailsDTO ToDetailsDTO(Question question, bool includeAnswers = false)
        {
            var questionDTO = new QuestionDetailsDTO
            {
                QuestionsId = question.QuestionsId,
                QuizId = question.QuizId,
                QuestionText = question.QuestionText
            };

            if (includeAnswers && question.Answers != null && question.Answers.Any())
            {
                questionDTO.Answers = question.Answers.Select(AnswerMapper.ToDTO).ToList();
            }

            return questionDTO;
        }

        public static List<QuestionDetailsDTO> ToDetailsDTOList(IEnumerable<Question> questions, bool includeAnswers = false)
        {
            return questions.Select(q => ToDetailsDTO(q, includeAnswers)).ToList();
        }

        public static Question ToEntity(int quizId, CreateQuestionDTO createQuestionDTO)
        {
            return new Question
            {
                QuizId = quizId,
                QuestionText = createQuestionDTO.QuestionText
            };
        }

        public static void UpdateEntity(Question question, UpdateQuestionDTO updateQuestionDTO)
        {
            question.QuestionText = updateQuestionDTO.QuestionText;
        }
    }
}
