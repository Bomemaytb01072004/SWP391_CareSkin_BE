namespace SWP391_CareSkin_BE.Models
{
    public class ProductPicture
    {
        public int ProductPictureId { get; set; }
        public int ProductId { get; set; }
        public string? PictureUrl { get; set; }
        public virtual Product Product { get; set; }
    }
}
