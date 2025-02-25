using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductUsage")]
    public class ProductUsage
    {
        public int ProductUsageId { get; set; }

        public int ProductId { get; set; }

        public int Step {  get; set; }

        public string Instruction { get; set; }

        public virtual Product Product { get; set; }
    }
}
