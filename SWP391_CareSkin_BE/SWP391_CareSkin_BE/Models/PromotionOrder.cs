using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("PromotionOrder")]
    public class PromotionOrder
    {
        public int PromotionOrderId { get; set; }

        public int OrderId { get; set; }

        public int PromotionId { get; set; }
    }
}
