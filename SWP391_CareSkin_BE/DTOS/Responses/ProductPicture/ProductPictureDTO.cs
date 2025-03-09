using System;

namespace SWP391_CareSkin_BE.DTOS.ProductPicture
{
    public class ProductPictureDTO
    {
        public int ProductPictureId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
