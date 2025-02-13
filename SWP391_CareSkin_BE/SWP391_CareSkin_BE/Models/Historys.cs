using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Historys")]
    public class Historys
    {
        public int HistoryId { get; set; }

        public int CustomerId { get; set; }

        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public virtual Customers Customers { get; set; }

        public virtual Questions Questions { get; set; }

        public virtual Answers Answers { get; set; }

    }
}
