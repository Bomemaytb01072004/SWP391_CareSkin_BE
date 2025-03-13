using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Routine
{
    public class RoutineProductUpdateRequestDTO
    {
        [Required]
        public int RoutineStepId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
