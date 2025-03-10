using SWP391_CareSkin_BE.DTOS.Requests.Quiz;
using SWP391_CareSkin_BE.DTOS.Responses.Quiz;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class QuizMapper
    {
        public static QuizDTO ToDTO(Quiz quiz, bool includeQuestions = false)
        {
            var quizDTO = new QuizDTO
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description
            };

            if (includeQuestions && quiz.Questions != null && quiz.Questions.Any())
            {
                quizDTO.Questions = quiz.Questions.Select(q => QuestionMapper.ToDTO(q)).ToList();
            }

            return quizDTO;
        }

        public static List<QuizDTO> ToDTOList(IEnumerable<Quiz> quizzes, bool includeQuestions = false)
        {
            return quizzes.Select(q => ToDTO(q, includeQuestions)).ToList();
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
