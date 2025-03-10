using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Quiz
{
    public class UpdateQuizDTO
    {
        [Required]
        public string Title { get; set; }
        
        public string? Description { get; set; }
    }
}
