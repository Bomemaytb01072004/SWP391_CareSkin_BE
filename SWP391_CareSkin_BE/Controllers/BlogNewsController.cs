using System.Data.Common;
using System.Security.Claims;
using FirebaseAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using SWP391_CareSkin_BE.DTOs.Requests.BlogNews;
using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.Services.Implementations;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _blogService;
        private readonly IFirebaseService _firebaseService;

        public BlogNewsController(IBlogNewsService blogService, IFirebaseService firebaseService)
        {
            _blogService = blogService;
            _firebaseService = firebaseService;
        }

        // GET: api/BlogNews
        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var blogs = await _blogService.GetAllNewsAsync();
            return Ok(blogs);
        }

        // GET: api/BlogNews/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _blogService.GetNewsByIdAsync(id);
            if (blog == null) return NotFound();

            return Ok(blog);
        }

        // POST: api/BlogNews
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromForm] BlogNewsCreateRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                int? adminId = null;
                int? staffId = null;

                if (userRoleClaim == "Admin" && int.TryParse(userIdClaim, out int parsedAdminId))
                {
                    adminId = parsedAdminId;
                }
                else if (userRoleClaim == "Staff" && int.TryParse(userIdClaim, out int parsedStaffId))
                {
                    staffId = parsedStaffId;
                }
                DateTime date = DateTime.Now;

                // Handle image upload
                string pictureUrl = null;
                if (request.PictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                    using var stream = request.PictureFile.OpenReadStream();
                    pictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                }

                var createdBlog = await _blogService.AddNewsAsync(request, date, pictureUrl, adminId, staffId);
                return CreatedAtAction(nameof(GetBlogById),
                    new { 
                        id = createdBlog.BlogId 
                    }, createdBlog);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex}"); 
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException}"); 
                    Console.WriteLine($"Inner Exception StackTrace: {ex.InnerException.StackTrace}");
                }
                return StatusCode(500, $"A database error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        // PUT: api/BlogNews/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromForm] BlogNewsUpdateRequest request)
        {
            try
            {
                string pictureUrl = null;
                if (request.PictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                    using var stream = request.PictureFile.OpenReadStream();
                    pictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
                }

                var updateBlog = await _blogService.UpdateNewsAsync(id, request, pictureUrl);
                if (updateBlog == null) return NotFound();

                return Ok(updateBlog);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException}");
                    Console.WriteLine($"Inner Exception StackTrace: {ex.InnerException.StackTrace}");
                }
                return StatusCode(500, $"A database error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }           
        }

        // DELETE: api/BlogNews/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var result = await _blogService.DeleteNewsAsync(id);
            if (!result)  return NotFound();

            return Ok(new { message = "Blog deleted successfully" });
        }

    }
}
