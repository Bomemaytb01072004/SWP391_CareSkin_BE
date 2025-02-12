using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("PromotionCustomer")]
    public class PromotionCustomer
    {
        public int PromotionCustomerId { get; set; }

        public int CustomerId { get; set; }

        public int PromotionId { get; set; }
    }
}
