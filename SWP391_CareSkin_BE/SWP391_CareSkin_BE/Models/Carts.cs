using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table ("Cart")]
    public class Carts
    {
        [Key]
        public int CartId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public virtual Products Product { get; set; }

        public virtual Customers Customers { get; set; }
    }
}
