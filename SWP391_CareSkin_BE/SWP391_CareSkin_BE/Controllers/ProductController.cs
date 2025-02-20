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
            // 1. Upload file nếu có
            string uploadedUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();

                // Lấy userId từ Supabase Auth (hoặc từ nguồn nào khác bạn đã triển khai)
                var userId = _supabaseService.GetCurrentUserId();

                uploadedUrl = await _supabaseService.UploadImageAsync(stream, fileName, userId);
            }

            // 2. Gọi service, truyền request + uploadedUrl
            var createdProduct = await _productService.CreateProductAsync(request, uploadedUrl);

            return CreatedAtAction(nameof(GetProductById),
                new { id = createdProduct.ProductId },
                createdProduct);
        }

        // PUT: api/Product/{id}
        // Nếu muốn cập nhật ảnh, cũng dùng [FromForm]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateRequestDTO request)
        {
            // 1. Nếu có file mới, upload file và lấy URL, kèm metadata owner
            string newPictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();

                // Lấy userId từ Supabase Auth (hoặc từ nguồn nào khác bạn đã triển khai)
                var userId = _supabaseService.GetCurrentUserId();

                newPictureUrl = await _supabaseService.UploadImageAsync(stream, fileName, userId);
            }

            // 2. Gọi service update, truyền newPictureUrl (nếu có)
            var updatedProduct = await _productService.UpdateProductAsync(id, request, newPictureUrl);
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
