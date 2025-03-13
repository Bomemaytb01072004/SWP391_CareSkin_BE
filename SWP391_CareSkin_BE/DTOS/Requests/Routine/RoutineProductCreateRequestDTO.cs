using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Routine
{
    public class RoutineProductCreateRequestDTO
    {
        [Required]
        public int RoutineStepId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
