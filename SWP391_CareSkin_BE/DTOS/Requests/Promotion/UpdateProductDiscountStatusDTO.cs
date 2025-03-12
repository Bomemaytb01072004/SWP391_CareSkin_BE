using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests.Promotion
{
    public class UpdateProductDiscountStatusDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int PromotionId { get; set; }

        public bool IsActive { get; set; }
    }
}
