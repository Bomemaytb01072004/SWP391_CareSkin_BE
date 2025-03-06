using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Helpers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;
using SWP391_CareSkin_BE.Services.Interfaces;



namespace SWP391_CareSkin_BE.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IStaffService _staffService;
        private readonly ICustomerService _customerService;

        public AuthController(IAdminService adminService, IStaffService staffService, ICustomerService customerService)
        {
            _adminService = adminService;
            _staffService = staffService;
            _customerService = customerService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var admin = await _adminService.Login(request);
            //var staff = await _staffService.Login(request);
            //var customer = await _customerService.Login(request);


            //string role = (string)admin?.Role ?? (string)staff?.Role ?? (string)customer?.Role;

            //switch (role)
            //{
            //    case "Admin":
            //        return Ok(admin);
            //    case "Staff":
            //        return Ok(staff);
            //    case "Customer":
            //        return Ok(customer);
            //    default:
            //        return BadRequest("Account and Role is not match");
            //}         

            LoginDTO loginDto = new LoginDTO
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var customer = await _customerService.Login(loginDto);
            if (customer != null) { return Ok(customer); }

            var admin = await _adminService.Login(loginDto);
            if (admin != null) { return Ok(admin); }

            var staff = await _staffService.Login(loginDto);
            if (staff != null) { return Ok(staff); }

            return BadRequest("Invalid username or password.");


        }

    }
}
