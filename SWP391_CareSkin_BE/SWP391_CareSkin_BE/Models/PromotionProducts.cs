using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("PromotionProduct")]
    public class PromotionProducts
    {
        [Key]
        public int ProductId { get; set; }

        [Key]
        public int PromotionId { get; set; }

        public virtual Products Product { get; set; }
        public virtual Promotions Promotion { get; set; }
    }
}
