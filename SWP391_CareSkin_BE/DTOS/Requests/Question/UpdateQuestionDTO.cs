using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Question
{
    public class UpdateQuestionDTO
    {
        [Required]
        public string QuestionText { get; set; }
    }
}
