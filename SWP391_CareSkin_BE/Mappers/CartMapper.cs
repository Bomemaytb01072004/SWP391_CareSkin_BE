using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class CartMapper
    {
        // Chuyển từ Cart Entity sang CartDTO
        public static CartDTO ToDTO(Cart cart)
        {
            if (cart == null)
                return null;

            return new CartDTO
            {
                CartId = cart.CartId,
                CustomerId = cart.CustomerId,
                ProductId = cart.ProductId,
                ProductVariationId = cart.ProductVariationId,
                Quantity = cart.Quantity,
                ProductName = cart.Product?.ProductName,
                Ml = cart.ProductVariation?.Ml ?? 0,
                Price = cart.ProductVariation?.Price ?? 0,
                TotalPrice = (cart.ProductVariation?.Price ?? 0) * cart.Quantity
            };
        }

        // Chuyển từ AddCartItemRequestDTO sang Cart Entity (dùng khi thêm mới)
        public static Cart ToEntity(CartCreateRequestDTO request)
        {
            if (request == null)
                return null;

            return new Cart
            {
                CustomerId = request.CustomerId,
                ProductId = request.ProductId,
                ProductVariationId = request.ProductVariationId,
                Quantity = request.Quantity
            };
        }
    }
}
