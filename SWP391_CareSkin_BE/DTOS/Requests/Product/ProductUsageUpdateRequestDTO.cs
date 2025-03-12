using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductUsageUpdateRequestDTO
    {
        public int ProductUsageId { get; set; }
        
        [Required(ErrorMessage = "Step number is required")]
        [Range(1, 100, ErrorMessage = "Step number must be between 1 and 100")]
        public int Step { get; set; }
        
        [Required(ErrorMessage = "Instruction is required")]
        [StringLength(500, ErrorMessage = "Instruction cannot exceed 500 characters")]
        public string Instruction { get; set; }
    }
}
