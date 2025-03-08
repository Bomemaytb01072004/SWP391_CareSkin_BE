using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.Quiz;
using SWP391_CareSkin_BE.DTOs.Responses.Quiz;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class QuizMapper
    {
        public static Quiz ToEntity(CreateQuizRequestDTO dto)
        {
            return new Quiz
            {
                Title = dto.Title,
                Description = dto.Description
            };
        }

        public static Quiz ToEntity(UpdateQuizRequestDTO dto, Quiz quiz)
        {
            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            return quiz;
        }

        public static QuizResponseDTO ToDTO(Quiz quiz)
        {
            return new QuizResponseDTO
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description
            };
        }
    }



}
