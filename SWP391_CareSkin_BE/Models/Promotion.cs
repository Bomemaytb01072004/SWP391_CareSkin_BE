using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Promotion")]
    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }

        public string PromotionName { get; set; }

        public double DiscountPercent;

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; } = new List<PromotionProduct>();
        public virtual ICollection<PromotionCustomer> PromotionCustomers { get; set; } = new List<PromotionCustomer>();
    }
}
