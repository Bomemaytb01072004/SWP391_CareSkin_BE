using SWP391_CareSkin_BE.DTOS.Responses.History;

namespace SWP391_CareSkin_BE.DTOS.Responses.UserQuizAttempt
{
    public class UserQuizAttemptDTO
    {
        public int UserQuizAttemptId { get; set; }
        public int CustomerId { get; set; }
        public int QuizId { get; set; }
        public DateTime AttemptDate { get; set; }
        public int AttemptNumber { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<HistoryDTO> Histories { get; set; }
    }
}
