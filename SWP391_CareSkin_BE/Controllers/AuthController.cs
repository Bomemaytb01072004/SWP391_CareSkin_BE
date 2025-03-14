using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "Auth", null, Request.Scheme)
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
            {
                var errorDetails = HttpContext.Request.Query["error"];
                Console.WriteLine($"Authentication failed. Error: {errorDetails}");

                return BadRequest($"Google authentication failed. Error: {errorDetails}");
            }

            var claims = authResult.Principal.Identities.FirstOrDefault()?.Claims
                .Select(c => new { c.Type, c.Value });

            return Ok(claims);
        }




    }
}
