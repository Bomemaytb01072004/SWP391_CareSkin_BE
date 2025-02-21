using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests.Admin;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
        public async Task<IActionResult> UpdateAdmin(int id, AdminUpdateRequestDTO request)
        {
            var updateAdmin = await _adminService.UpdateAdminAsync(request, id);
            if(updateAdmin == null)
            {
                return NotFound();
            }
            return Ok(updateAdmin);
        }
      







    }
}
