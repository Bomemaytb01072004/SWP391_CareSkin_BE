using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Quizs")]
    public class Quizs
    {
        public int QuizId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Questions> Questions { get; set; } = new List<Questions>();
        public virtual ICollection<Results> Results { get; set; } = new List<Results>();
    }
}
