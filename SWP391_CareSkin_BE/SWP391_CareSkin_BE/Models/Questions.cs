using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Questions")]
    public class Questions
    {
        public int QuestionsId { get; set; }

        public int QuizId { get; set; }

        public string QuestionContext { get; set; }

        public virtual Quizs Quizs { get; set; }

        public virtual ICollection<Answers> Answers { get; set; } = new List<Answers>();

        public virtual ICollection<Historys> Historys { get; set; } = new List<Historys>();
    }
}
