using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Questions")]
    public class Questions
    {
        public int QuestionsID { get; set; }

        public int QuizId { get; set; }

        public string QuestionContext { get; set; }
    }
}
