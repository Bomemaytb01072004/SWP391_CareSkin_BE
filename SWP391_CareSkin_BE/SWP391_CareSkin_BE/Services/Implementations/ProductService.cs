using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupabaseService _supabaseService;

        public ProductService(IProductRepository productRepository, ISupabaseService supabaseService)
        {
            _productRepository = productRepository;
            _supabaseService = supabaseService;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(ProductMapper.ToDTO).ToList();
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(product);
        }

        // Tạo sản phẩm với pictureUrl
        public async Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request, string pictureUrl)
        {
            // Dùng mapper
            var productEntity = ProductMapper.ToEntity(request, pictureUrl);

            await _productRepository.AddProductAsync(productEntity);
            var createdProduct = await _productRepository.GetProductByIdAsync(productEntity.ProductId);

            return ProductMapper.ToDTO(createdProduct);
        }


        // Update sản phẩm với pictureUrl
        public async Task<ProductDTO> UpdateProductAsync(int productId, ProductUpdateRequestDTO request, string pictureUrl)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null) return null;

            ProductMapper.UpdateEntity(existingProduct, request, pictureUrl);
            await _productRepository.UpdateProductAsync(existingProduct);

            var updatedProduct = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
                return false;

            // Nếu sản phẩm có ảnh, xóa ảnh khỏi Supabase
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                var fileName = product.PictureUrl.Split('/').Last();
                await _supabaseService.DeleteImageAsync(fileName);
            }

            await _productRepository.DeleteProductAsync(productId);
            return true;
        }
    }
}
