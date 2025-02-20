using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ISupabaseService _supabaseService; // Dịch vụ upload ảnh

        public ProductController(IProductService productService, ISupabaseService supabaseService)
        {
            _productService = productService;
            _supabaseService = supabaseService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST: api/Product
        // Lưu ý: Để nhận file, dùng [FromForm] thay vì [FromBody]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequestDTO request)
        {
            // 1. Upload ảnh lên Supabase (nếu có file)
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();
                // Upload file lên Supabase và lấy URL
                var uploadedUrl = await _supabaseService.UploadImageAsync(stream, fileName);

                // Gán URL vừa upload vào DTO, để Service map sang Entity
                request.PictureUrl = uploadedUrl;
            }

            // 2. Gọi service, truyền DTO (đã có PictureUrl nếu có ảnh)
            var createdProduct = await _productService.CreateProductAsync(request);

            // 3. Trả về DTO
            return CreatedAtAction(nameof(GetProductById),
                new { id = createdProduct.ProductId },
                createdProduct);
        }

        // PUT: api/Product/{id}
        // Nếu muốn cập nhật ảnh, cũng dùng [FromForm]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateRequestDTO request)
        {
            // 1. Upload ảnh mới nếu có
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();
                var newPictureUrl = await _supabaseService.UploadImageAsync(stream, fileName);

                // Gán vào DTO để Service biết cập nhật
                request.PictureUrl = newPictureUrl;
            }

            // 2. Gọi Service cập nhật
            var updatedProduct = await _productService.UpdateProductAsync(productId: id, request);

            // 3. Nếu không tồn tại sản phẩm => 404
            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { message = "Product deleted successfully" });
        }
    }
}
