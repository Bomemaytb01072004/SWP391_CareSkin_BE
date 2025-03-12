using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Routine
{
    public class RoutineProductCreateRequestDTO
    {
        [Required]
        public int RoutineId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
