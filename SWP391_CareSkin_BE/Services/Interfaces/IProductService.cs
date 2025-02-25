using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request);
        Task<ProductDTO> UpdateProductAsync(int productId, ProductUpdateRequestDTO request);
        Task<bool> DeleteProductAsync(int productId);
    }
}
