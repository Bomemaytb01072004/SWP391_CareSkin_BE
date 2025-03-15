using SWP391_CareSkin_BE.DTOS.Responses.Question;

namespace SWP391_CareSkin_BE.DTOS.Responses.Quiz
{
    public class QuizDTO
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
