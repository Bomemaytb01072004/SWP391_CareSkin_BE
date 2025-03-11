using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.DTOS.Requests.Quiz;
using SWP391_CareSkin_BE.DTOS.Responses.Quiz;
using SWP391_CareSkin_BE.Models;
using System.Linq;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class QuizMapper
    {
        public static QuizDTO ToDTO(Quiz quiz)
        {
            var quizDTO = new QuizDTO
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description
            };

            return quizDTO;
        }

        public static QuizDetailsDTO ToDetailsDTO(Quiz quiz)
        {
            var quizDetailsDTO = new QuizDetailsDTO
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description
            };

            if (quiz.Questions != null && quiz.Questions.Any())
            {
                // Explicitly load questions with their answers
                quizDetailsDTO.Questions = quiz.Questions.Select(q => {
                    var questionDTO = QuestionMapper.ToDetailsDTO(q, true); // true to include answers
                    return questionDTO;
                }).ToList();
            }

            return quizDetailsDTO;
        }

        public static List<QuizDTO> ToDTOList(IEnumerable<Quiz> quizzes)
        {
            return quizzes.Select(q => ToDTO(q)).ToList();
        }

        public static Quiz ToEntity(CreateQuizDTO createQuizDTO)
        {
            return new Quiz
            {
                Title = createQuizDTO.Title,
                Description = createQuizDTO.Description
            };
        }

        public static void UpdateEntity(Quiz quiz, UpdateQuizDTO updateQuizDTO)
        {
            quiz.Title = updateQuizDTO.Title;
            quiz.Description = updateQuizDTO.Description;
        }
    }
}
