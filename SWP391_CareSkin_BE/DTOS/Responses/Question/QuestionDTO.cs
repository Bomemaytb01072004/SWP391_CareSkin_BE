using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOs.Responses.Question
{
    public class QuestionDTO
    {
        public int QuestionsId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; }
    }
}
