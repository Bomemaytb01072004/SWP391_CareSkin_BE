using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("History")]
    public class History
    {
        public int HistoryId { get; set; }
        
        public int AttemmptId { get; set; }

        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public virtual UserQuizAttempt UserQuizAttempt { get; set; }

        public virtual Question Question { get; set; }

        public virtual Answer Answer { get; set; }

    }
}
