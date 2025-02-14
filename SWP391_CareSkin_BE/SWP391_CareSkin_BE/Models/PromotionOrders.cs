using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("PromotionOrder")]
    public class PromotionOrders
    {
        public int OrderId { get; set; }

        public int PromotionId { get; set; }

        public virtual Promotions Promotions { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
