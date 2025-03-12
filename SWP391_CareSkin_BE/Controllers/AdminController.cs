using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests.Admin;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.RatingFeedback;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IFirebaseService _firebaseService;
        private readonly IRatingFeedbackService _ratingFeedbackService;

        public AdminController(IAdminService adminService, IFirebaseService firebaseService, IRatingFeedbackService ratingFeedbackService)
        {
            _adminService = adminService;
            _firebaseService = firebaseService;
            _ratingFeedbackService = ratingFeedbackService;
        }

        private int GetAdminIdFromClaims()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "AdminId");
            if (idClaim == null)
                return 0;

            if (int.TryParse(idClaim.Value, out int adminId))
                return adminId;

            return 0;
        }

        // GET: api/Admin 
        [HttpGet]
        public async Task<IActionResult> GetAdmin()
        {
            var adminList = await _adminService.GetAdminAsync();
            return Ok(adminList);
        }

        // GET: api/Admin/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromForm] AdminUpdateRequestDTO request)
        {
            // Check if admin exists
            var adminList = await _adminService.GetAdminAsync();
            var adminExists = adminList.Any(a => a.AdminId == id);
            if (!adminExists)
            {
                return NotFound("Admin not found");
            }
            
            // 1. Nu1ebfu cu00f3 file mu1edbi, upload file vu00e0 lu1ea5y URL
            string newPictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();

                newPictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
            }

            var updateAdmin = await _adminService.UpdateAdminAsync(request, id, newPictureUrl);
            if(updateAdmin == null)
            {
                return NotFound();
            }
            return Ok(updateAdmin);
        }

        // Rating Feedback Admin Methods
        // GET: api/Admin/RatingFeedback
        [HttpGet("RatingFeedback")]
        public async Task<IActionResult> GetAllRatingFeedbacks()
        {
            var ratingFeedbacks = await _ratingFeedbackService.GetAllRatingFeedbacksAsync();
            return Ok(ratingFeedbacks);
        }

        // GET: api/Admin/RatingFeedback/{id}
        [HttpGet("RatingFeedback/{id}")]
        public async Task<IActionResult> GetRatingFeedbackById(int id)
        {           
            var ratingFeedback = await _ratingFeedbackService.GetRatingFeedbackByIdAsync(id);
            if (ratingFeedback == null)
                return NotFound();

            return Ok(ratingFeedback);
        }

        // PUT: api/Admin/RatingFeedback/{id}/visibility
        [HttpPut("RatingFeedback/{id}/visibility")]
        public async Task<IActionResult> ToggleRatingFeedbackVisibility(int id, [FromBody] AdminRatingFeedbackActionDTO actionDto)
        {            
            var result = await _ratingFeedbackService.AdminToggleRatingFeedbackVisibilityAsync(id, actionDto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Admin/RatingFeedback/{id}
        [HttpDelete("RatingFeedback/{id}")]
        public async Task<IActionResult> DeleteRatingFeedback(int id)
        {
            //// Verify admin/staff exists
            //int adminId = GetAdminIdFromClaims();
            //var adminList = await _adminService.GetAdminAsync();
            //var adminExists = adminList.Any(a => a.AdminId == adminId);
            //if (!adminExists)
            //{
            //    return Unauthorized("Admin not found");
            //}
            
            var result = await _ratingFeedbackService.AdminDeleteRatingFeedbackAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
