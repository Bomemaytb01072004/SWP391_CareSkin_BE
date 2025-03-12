using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Routine
{
    public class RoutineCreateRequestDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Routine name must be between 3 and 100 characters")]
        public string RoutineName { get; set; }

        [Required]
        [RegularExpression("^(morning|evening)$", ErrorMessage = "RoutinePeriod must be either 'morning' or 'evening'")]
        public string RoutinePeriod { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters")]
        public string Description { get; set; }

        [Required]
        public int SkinTypeId { get; set; }
    }
}
