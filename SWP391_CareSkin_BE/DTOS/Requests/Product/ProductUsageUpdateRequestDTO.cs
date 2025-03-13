using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOs.Requests.Product
{
    public class ProductUsageUpdateRequestDTO
    {
        public int ProductUsageId { get; set; }
        
        [Required(ErrorMessage = "Step number is required")]
        public int Step { get; set; }
        
        [Required(ErrorMessage = "Instruction is required")]
        public string Instruction { get; set; }
    }
}
