using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Routine
{
    public class RoutineStepUpdateRequestDTO
    {
        [Required]
        public int RoutineProductId { get; set; }
        
        [Required]
        public int StepOrder { get; set; }
        
        [Required]
        [StringLength(100)]
        public string StepName { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
    }
}
