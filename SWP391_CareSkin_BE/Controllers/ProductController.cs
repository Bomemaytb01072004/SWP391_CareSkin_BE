using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
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
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<ProductDTO>>> GetActiveProducts()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        [HttpGet("inactive")]
        public async Task<ActionResult<List<ProductDTO>>> GetInactiveProducts()
        {
            var products = await _productService.GetInactiveProductsAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo các tiêu chí lọc (Keyword, Category, Brand, Price, ML, Sort, Pagination)
        /// </summary>
        /// <param name="request">Các tham số lọc sản phẩm</param>
        /// <returns>Danh sách sản phẩm và tổng số sản phẩm phù hợp</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchRequestDTO request)
        {
            var (products, totalCount) = await _productService.SearchProductsAsync(request);

            return Ok(new
            {
                TotalCount = totalCount,
                Products = products
            });
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromForm] ProductCreateRequestDTO request)
        {
            try
            {
                // Handle image upload
                string pictureUrl = null;
                if (request.PictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                    using var stream = request.PictureFile.OpenReadStream();
                    pictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                }

                var product = await _productService.CreateProductAsync(request, pictureUrl);
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the product");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, [FromForm] ProductUpdateRequestDTO request)
        {
            try
            {
                // Handle image upload if new image is provided
                string pictureUrl = null;
                if (request.PictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                    using var stream = request.PictureFile.OpenReadStream();
                    pictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                }

                var product = await _productService.UpdateProductAsync(id, request, pictureUrl);
                if (product == null)
                    return NotFound($"Product with ID {id} not found");

                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the product");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                    return NotFound($"Product with ID {id} not found");

                return Ok("Delete product successful");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the product");
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<string>>> GetCategories()
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = products.Select(p => p.Category).Distinct().ToList();
            return Ok(categories);
        }

        [HttpDelete("usage/{id}")]
        public async Task<IActionResult> DeleteProductUsage(int id)
        {
            var result = await _productService.DeleteProductUsageAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("skin-type/{id}")]
        public async Task<IActionResult> DeleteProductForSkinType(int id)
        {
            var result = await _productService.DeleteProductForSkinTypeAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("variation/{id}")]
        public async Task<IActionResult> DeleteProductVariation(int id)
        {
            var result = await _productService.DeleteProductVariationAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("main-ingredient/{id}")]
        public async Task<IActionResult> DeleteProductMainIngredient(int id)
        {
            var result = await _productService.DeleteProductMainIngredientAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("detail-ingredient/{id}")]
        public async Task<IActionResult> DeleteProductDetailIngredient(int id)
        {
            var result = await _productService.DeleteProductDetailIngredientAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
