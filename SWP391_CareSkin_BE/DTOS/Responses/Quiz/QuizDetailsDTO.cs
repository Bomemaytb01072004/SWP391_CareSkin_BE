using SWP391_CareSkin_BE.DTOS.Responses.Question;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOs.Responses.Quiz
{
    public class QuizDetailsDTO
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<QuestionDetailsDTO> Questions { get; set; } = new List<QuestionDetailsDTO>();
    }
}
