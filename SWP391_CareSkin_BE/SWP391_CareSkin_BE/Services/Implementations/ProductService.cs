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

        public async Task<ProductDTO> CreateProductAsync(ProductCreateRequestDTO request)
        {
            // Kiểm tra nếu có file ảnh được gửi từ client
            string pictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                // Đặt tên file duy nhất để tránh trùng lặp
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();
                // Upload file lên Supabase và lấy URL trả về
                pictureUrl = await _supabaseService.UploadImageAsync(stream, fileName);
            }

            // Chuyển từ DTO sang entity, truyền pictureUrl (có thể null nếu không có ảnh)
            var productEntity = ProductMapper.ToEntity(request, pictureUrl);
            await _productRepository.AddProductAsync(productEntity);

            // Sau khi lưu, lấy lại sản phẩm (có thể load các quan hệ nếu cần)
            var createdProduct = await _productRepository.GetProductByIdAsync(productEntity.ProductId);
            return ProductMapper.ToDTO(createdProduct);
        }

        public async Task<ProductDTO> UpdateProductAsync(int productId, ProductUpdateRequestDTO request)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null)
                return null;

            string newPictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                // Nếu có ảnh cũ, xóa ảnh đó khỏi Supabase
                if (!string.IsNullOrEmpty(existingProduct.PictureUrl))
                {
                    var oldFileName = existingProduct.PictureUrl.Split('/').Last();
                    await _supabaseService.DeleteImageAsync(oldFileName);
                }

                // Upload ảnh mới
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();
                newPictureUrl = await _supabaseService.UploadImageAsync(stream, fileName);
            }

            // Cập nhật thông tin sản phẩm, nếu newPictureUrl không null thì sẽ cập nhật lại trường PictureUrl
            ProductMapper.UpdateEntity(existingProduct, request, newPictureUrl);
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
