using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductForSkinType")]
    public class ProductForSkinType
    {
        [Key]
        public int ProductForSkinTypeId { get; set; }

        public int ProductId { get; set; }
        public int SkinTypeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual SkinType SkinType { get; set; }
    }
}
