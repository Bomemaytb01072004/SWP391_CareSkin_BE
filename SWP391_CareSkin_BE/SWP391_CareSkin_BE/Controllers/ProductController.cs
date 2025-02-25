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
        private readonly IFirebaseService _firebaseService;

        public ProductController(IProductService productService, IFirebaseService firebaseService)
        {
            _productService = productService;
            _firebaseService = firebaseService;
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
        // Lưu ý: Để nhận file, dùng [FromForm]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequestDTO request)
        {
            // 1. Upload file nếu có
            string uploadedUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();

                // Gọi FirebaseService để upload
                uploadedUrl = await _firebaseService.UploadImageAsync(stream, fileName);
            }

            // 2. Gọi service, truyền request + uploadedUrl
            var createdProduct = await _productService.CreateProductAsync(request, uploadedUrl);

            return CreatedAtAction(nameof(GetProductById),
                new { id = createdProduct.ProductId },
                createdProduct);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateRequestDTO request)
        {
            // 1. Kiểm tra id hợp lệ
            if (id <= 0)
                return BadRequest(new { error = "Invalid product ID." });

            // 2. Nếu có file mới, upload file và lấy URL, nếu không thì giữ ảnh cũ
            string? newPictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();
                newPictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
            }

            // 3. Xử lý giá trị mặc định cho Variations nếu null
            request.Variations ??= new List<ProductVariationCreateRequestDTO>();

            // 4. Gọi service update
            var updatedProduct = await _productService.UpdateProductAsync(id, request, newPictureUrl);

            if (updatedProduct == null)
                return NotFound(new { error = "Product not found." });

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
