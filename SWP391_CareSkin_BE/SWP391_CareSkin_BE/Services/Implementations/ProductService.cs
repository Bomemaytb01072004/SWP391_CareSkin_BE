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
        
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
        
        public async Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request)
        {
            var productEntity = ProductMapper.ToEntity(request);
            await _productRepository.AddProductAsync(productEntity);
            // Sau khi lưu, lấy lại sản phẩm có đầy đủ quan hệ nếu cần
            var createdProduct = await _productRepository.GetProductByIdAsync(productEntity.ProductId);
            return ProductMapper.ToDTO(createdProduct);
        }
        
        public async Task<ProductDTO> UpdateProductAsync(int productId, ProductUpdateRequestDTO request)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if(existingProduct == null) return null;
            
            ProductMapper.UpdateEntity(existingProduct, request);
            await _productRepository.UpdateProductAsync(existingProduct);
            
            var updatedProduct = await _productRepository.GetProductByIdAsync(productId);
            return ProductMapper.ToDTO(updatedProduct);
        }
        
        public async Task<bool> DeleteProductAsync(int productId)
        {
            await _productRepository.DeleteProductAsync(productId);
            return true;
        }
    }
}
