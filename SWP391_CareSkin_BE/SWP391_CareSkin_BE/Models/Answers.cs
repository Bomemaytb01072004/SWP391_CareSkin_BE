using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Answers")]
    public class Answers
    {
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        public int AnswersContext { get; set; }

        public int PointForSkinType { get; set; }


    }
}
