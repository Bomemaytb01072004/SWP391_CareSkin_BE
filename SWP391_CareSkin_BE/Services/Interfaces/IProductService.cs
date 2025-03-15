using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<List<ProductDTO>> GetActiveProductsAsync();
        Task<List<ProductDTO>> GetInactiveProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request, string pictureUrl);
        Task<ProductDTO> UpdateProductAsync(int productId, ProductUpdateRequestDTO request, string pictureUrl);
        Task<bool> DeleteProductAsync(int productId);
        Task<(List<ProductDTO> Products, int TotalCount)> SearchProductsAsync(ProductSearchRequestDTO request);
        Task<bool> DeleteProductUsageAsync(int id);
        Task<bool> DeleteProductForSkinTypeAsync(int id);
        Task<bool> DeleteProductVariationAsync(int id);
        Task<bool> DeleteProductMainIngredientAsync(int id);
        Task<bool> DeleteProductDetailIngredientAsync(int id);
    }
}
