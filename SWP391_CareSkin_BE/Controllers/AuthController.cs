using System.Security.Claims;
using System.IO;
using System.Text.Json;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
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
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;
        private readonly IWebHostEnvironment _environment;

        public AuthController(IAdminService adminService, IStaffService staffService, ICustomerService customerService, 
            IConfiguration configuration, JwtHelper jwtHelper, IWebHostEnvironment environment)
        {
            _adminService = adminService;
            _staffService = staffService;
            _customerService = customerService;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
            _environment = environment;
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

        private string GetGoogleClientId()
        {
            try
            {
                string filePath = Path.Combine(_environment.ContentRootPath, "googleauth.json");
                if (System.IO.File.Exists(filePath))
                {
                    string jsonContent = System.IO.File.ReadAllText(filePath);
                    using JsonDocument doc = JsonDocument.Parse(jsonContent);
                    return doc.RootElement.GetProperty("GoogleAuth").GetProperty("ClientId").GetString();
                }
                return _configuration["Authentication:Google:ClientId"]; // Fallback to appsettings.json
            }
            catch (Exception)
            {
                // If there's any error reading the file, fall back to appsettings.json
                return _configuration["Authentication:Google:ClientId"];
            }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO request)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { GetGoogleClientId() }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token, settings);

                if (payload == null)
                {
                    return BadRequest("Invalid Google token.");
                }

                // Check if user already exists in database
                var existingUser = await _customerService.GetCustomerByEmailAsync(payload.Email);
                
                if (existingUser == null)
                {
                    // Create a new account if it doesn't exist
                    // Generate a random password for Google users
                    string randomPassword = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());
                    
                    var newUser = new Customer
                    {
                        Email = payload.Email,
                        UserName = payload.Email.Split('@')[0], // Use part of email as username
                        Password = randomPassword,
                        FullName = payload.Name,
                        PictureUrl = payload.Picture,
                        // Other fields can be populated as needed
                    };

                    var createdUser = await _customerService.CreateGoogleUserAsync(newUser);
                    existingUser = await _customerService.GetCustomerByEmailAsync(payload.Email);
                }

                // Generate JWT Token
                string role = "User";
                var jwtToken = _jwtHelper.GenerateToken(existingUser.UserName, role);

                return Ok(new
                {
                    token = jwtToken,
                    user = new
                    {
                        id = existingUser.CustomerId,
                        email = existingUser.Email,
                        userName = existingUser.UserName,
                        fullName = existingUser.FullName,
                        pictureUrl = existingUser.PictureUrl,
                        role = role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing Google login: {ex.Message}");
            }
        }
    }
}
