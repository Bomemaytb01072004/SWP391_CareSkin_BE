using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table ("Cart")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int Customer_Id { get; set; }

        public int Product_Id { get; set; }

        public virtual Product Product { get; set; }
    }
}
