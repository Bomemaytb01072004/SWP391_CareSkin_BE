using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Routine
{
    public class RoutineRecommendationRequestDTO
    {
        [Required]
        public int SkinTypeId { get; set; }
        
        [Required]
        [RegularExpression("^(morning|evening)$", ErrorMessage = "Period must be either 'morning' or 'evening'")]
        public string Period { get; set; }
    }
}
