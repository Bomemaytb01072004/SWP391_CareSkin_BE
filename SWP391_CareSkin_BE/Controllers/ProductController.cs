﻿using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "Admin")]
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
            {
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
