using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Quiz")]
    public class Quiz
    {
        public int QuizId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
