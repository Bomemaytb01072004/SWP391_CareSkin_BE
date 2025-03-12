using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.DTOs.Responses
{
    public class CartDTO
    {
        public int CartId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int ProductVariationId { get; set; }

        public int Quantity { get; set; }

        public string ProductName { get; set; }

        public int Ml { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }
        
        public decimal SalePrice { get; set; }
        
        public decimal TotalSalePrice { get; set; }
        
        public bool IsOnSale { get; set; }

        public List<ProductVariationDTO> ProductVariations { get; set; }
    }
}
