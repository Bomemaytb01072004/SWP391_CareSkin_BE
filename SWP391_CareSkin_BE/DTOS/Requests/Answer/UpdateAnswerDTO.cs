using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Answer
{
    public class UpdateAnswerDTO
    {
        [Required]
        public string AnswersText { get; set; }
        
        [Required]
        public int Score { get; set; }
    }
}
