using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IFirebaseService _firebaseService;

        public BrandController(IBrandService brandService, IFirebaseService firebaseService)
        {
            _brandService = brandService;
            _firebaseService = firebaseService;
        }

        // GET: api/Brand
        [HttpGet]
        public async Task<ActionResult<List<BrandDTO>>> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<BrandDTO>>> GetActiveBrands()
        {
            var brands = await _brandService.GetActiveBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("inactive")]
        public async Task<ActionResult<List<BrandDTO>>> GetInactiveBrands()
        {
            var brands = await _brandService.GetInactiveBrandsAsync();
            return Ok(brands);
        }

        // GET: api/Brand/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }

        // POST: api/Brand
        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromForm] BrandCreateRequestDTO request)
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

                var createdBrand = await _brandService.CreateBrandAsync(request, pictureUrl);
                return CreatedAtAction(nameof(GetBrandById),
                    new { id = createdBrand.BrandId }, createdBrand);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the brand: {ex.Message}");
            }
        }

        // PUT: api/Brand/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandUpdateRequestDTO request)
        {
            try
            {
                // Handle image upload if a new image is provided
                string pictureUrl = null;
                if (request.PictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                    using var stream = request.PictureFile.OpenReadStream();
                    pictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                }

                var updatedBrand = await _brandService.UpdateBrandAsync(id, request, pictureUrl);
                if (updatedBrand == null)
                    return NotFound();
                return Ok(updatedBrand);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the brand: {ex.Message}");
            }
        }

        // DELETE: api/Brand/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var result = await _brandService.DeleteBrandAsync(id);
            if (!result)
                return NotFound();
            return Ok(new { message = "Brand deleted successfully" });
        }
    }
}
