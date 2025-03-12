using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Answer")]
    public class Answer
    {
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }
        
        public string AnswersText { get; set; }

        public int Score { get; set; }

        public virtual Question Question { get; set; }

        public virtual ICollection<History> Historys { get; set; } = new List<History>();

    }
}
