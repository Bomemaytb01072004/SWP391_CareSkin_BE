using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class QuizMapper
    {
        public static QuizDTO ToDTO(Quiz quiz)
        {
            return new QuizDTO
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description
            };
        }

        public static Quiz ToEntity(QuizDTO dto)
        {
            return new Quiz
            {
                QuizId = dto.QuizId,
                Title = dto.Title,
                Description = dto.Description
            };
        }
    }
}
