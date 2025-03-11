using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("UserQuizAttempt")]
    public class UserQuizAttempt
    {
        public int UserQuizAttemptId { get; set; }
        public int CustomerId { get; set; }
        public int QuizId { get; set; }
        public DateOnly AttemptDate { get; set; }
        public int AttemptNumber { get; set; }
        public bool IsCompleted { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Quiz Quiz { get; set; }
        public ICollection<History> Histories { get; set; } = new List<History>();
    }
}
