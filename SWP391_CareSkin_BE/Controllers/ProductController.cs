﻿using Microsoft.AspNetCore.Authorization;
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

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginatedResponse<ProductDTO>>> SearchProducts([FromQuery] ProductSearchRequestDTO request)
        {
            var (products, totalCount) = await _productService.SearchProductsAsync(request);

            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 10;

            var response = new PaginatedResponse<ProductDTO>
            {
                Items = products,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };

            return Ok(response);
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
        [Authorize(Roles = "Admin")]
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

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                    return NotFound($"Product with ID {id} not found");

                return NoContent();
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
