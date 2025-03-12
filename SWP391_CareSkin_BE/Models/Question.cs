using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Question")]
    public class Question
    {
        public int QuestionsId { get; set; }

        public int QuizId { get; set; }

        public string QuestionText { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public virtual ICollection<History> Historys { get; set; } = new List<History>();
    }
}
