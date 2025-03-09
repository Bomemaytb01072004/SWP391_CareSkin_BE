using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ProductPicture")]
    public class ProductPicture
    {
        [Key]
        public int ProductPictureId { get; set; }

        public int ProductId { get; set; }

        public string? PictureUrl { get; set; }

        public virtual Product Product { get; set; }
    }
}
