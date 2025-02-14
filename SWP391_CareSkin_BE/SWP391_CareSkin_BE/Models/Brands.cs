using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Brand")]
    public class Brands
    {
        [Key]
        public int BrandId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Products> Products { get; set; } = new List<Products>();
    }
}
