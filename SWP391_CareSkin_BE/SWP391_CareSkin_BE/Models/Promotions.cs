using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Promotion")]
    public class Promotions
    {
        [Key]
        public int PromotionId { get; set; }

        public string Name { get; set; }

        public decimal Discount_percent;

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public virtual ICollection<PromotionProducts> PromotionProducts { get; set; } = new List<PromotionProducts>();
        public virtual ICollection<PromotionOrders> PromotionOrders { get; set; } = new List<PromotionOrders>();
    }
}
