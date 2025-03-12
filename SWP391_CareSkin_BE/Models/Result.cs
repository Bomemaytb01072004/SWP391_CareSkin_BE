using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Result")]
    public class Result
    {
        public int ResultId { get; set; }

        public int CustomerId { get; set; }

        public int QuizId { get; set; }

        public int SkinTypeId { get; set; }

        public int TotalScore { get; set; }

        public DateTime LastQuizTime { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual SkinType SkinType { get; set; }
    }
}
