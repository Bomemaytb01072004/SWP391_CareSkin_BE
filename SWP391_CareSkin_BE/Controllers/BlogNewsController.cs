using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // GET: api/BlogNews/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetBlogByTitle(string title)
        {
            var blog = await _blogService.GetNewsByNameAsync(title);
            if (blog == null) return NotFound();

            return Ok(blog);
        }

        // POST: api/BlogNews
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromForm] BlogNewsCreateRequest request)
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

                var createdBlog = await _blogService.AddNewsAsync(request, pictureUrl);
                return CreatedAtAction(nameof(GetBlogById),
                    new { id = createdBlog.BlogId }, createdBlog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the blog: {ex.Message}");
            }
        }

        // PUT: api/BlogNews/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogNewsUpdateRequest request)
        {
            var updateBlog = await _blogService.UpdateNewsAsync(id, request);
            if (updateBlog == null) return NotFound();

            return Ok(updateBlog);
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
