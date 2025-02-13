using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Results")]
    public class Results
    {
        public int ResultId { get; set; }

        public int CustomerId { get; set; }

        public int QuizId { get; set; }

        public int SkinTypeId { get; set; }

        public DateTime LastQuizTime { get; set; }

        public virtual Customers Customers { get; set; }

        public virtual Quizs Quizs { get; set; }

        public virtual SkinType SkinType { get; set; }
    }
}
