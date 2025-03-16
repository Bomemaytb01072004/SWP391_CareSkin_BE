using System.Security.Claims;
using System.IO;
using System.Text.Json;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
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
using System.Net.Http;
using System.Text;

namespace SWP391_CareSkin_BE.Controllers
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
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IAdminService adminService, IStaffService staffService, ICustomerService customerService, 
            IConfiguration configuration, JwtHelper jwtHelper, IWebHostEnvironment environment, IHttpClientFactory httpClientFactory)
        {
            _adminService = adminService;
            _staffService = staffService;
            _customerService = customerService;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
            _environment = environment;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }    

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

        #region === GOOGLE LOGIN (REDIRECT FLOW) ===

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            // Gọi Challenge để redirect user tới Google
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

            // Lấy claim từ Google
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
                string role = "Customer";
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

        #endregion

        #region === FACEBOOK LOGIN (TOKEN FLOW) ===

        private string GetFacebookAppId()
        {
            try
            {
                string filePath = Path.Combine(_environment.ContentRootPath, "facebookkey.json");
                if (System.IO.File.Exists(filePath))
                {
                    string jsonContent = System.IO.File.ReadAllText(filePath);
                    using JsonDocument doc = JsonDocument.Parse(jsonContent);
                    return doc.RootElement.GetProperty("FacebookAuth").GetProperty("AppId").GetString();
                }
                return _configuration["Authentication:Facebook:AppId"]; 
            }
            catch (Exception)
            {
                return _configuration["Authentication:Facebook:AppId"]; 
            }
        }

        private string GetFacebookAppSecret()
        {
            try
            {
                string filePath = Path.Combine(_environment.ContentRootPath, "facebookkey.json");
                if (System.IO.File.Exists(filePath))
                {
                    string jsonContent = System.IO.File.ReadAllText(filePath);
                    using JsonDocument doc = JsonDocument.Parse(jsonContent);
                    return doc.RootElement.GetProperty("FacebookAuth").GetProperty("AppSecret").GetString();
                }
                return _configuration["Authentication:Facebook:AppSecret"]; 
            }
            catch (Exception)
            {
                return _configuration["Authentication:Facebook:AppSecret"]; 
            }
        }

        [HttpGet("facebook-login")]
        public IActionResult FacebookLogin()
        {
            // Gọi Challenge để redirect user tới Facebook
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookCallback", "Auth", null, Request.Scheme)
            };

            // Thêm param "auth_type=reauthenticate"
            properties.Parameters["auth_type"] = "reauthenticate";

            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-facebook")]
        public async Task<IActionResult> FacebookCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authResult.Succeeded)
            {
                var errorDetails = HttpContext.Request.Query["error"];
                Console.WriteLine($"Authentication failed. Error: {errorDetails}");

                return BadRequest($"Facebook authentication failed. Error: {errorDetails}");
            }

            // Lấy claim từ Facebook
            var claims = authResult.Principal.Identities.FirstOrDefault()?.Claims
                .Select(c => new { c.Type, c.Value });

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email);
            var name = authResult.Principal.FindFirstValue(ClaimTypes.Name);
            var id = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user already exists in database
            var existingUser = await _customerService.GetCustomerByEmailAsync(email);
            
            if (existingUser == null)
            {
                // Create a new account if it doesn't exist
                string randomPassword = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());
                
                var newUser = new Customer
                {
                    Email = email,
                    UserName = email.Split('@')[0], // Use part of email as username
                    Password = randomPassword,
                    FullName = name,
                };

                var createdUser = await _customerService.CreateGoogleUserAsync(newUser);
                existingUser = await _customerService.GetCustomerByEmailAsync(email);
            }

            // Generate JWT Token
            string role = "Customer";
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

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginDTO request)
        {
            try
            {
                // Verify the token with Facebook
                var client = _httpClientFactory.CreateClient();
                var appId = GetFacebookAppId();
                var appSecret = GetFacebookAppSecret();
                
                // First, verify the token with Facebook
                var verifyTokenUrl = $"https://graph.facebook.com/debug_token?input_token={request.Token}&access_token={appId}|{appSecret}";
                var verifyResponse = await client.GetAsync(verifyTokenUrl);
                
                if (!verifyResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Invalid Facebook token.");
                }
                
                var verifyContent = await verifyResponse.Content.ReadAsStringAsync();
                using var verifyDoc = JsonDocument.Parse(verifyContent);
                var isValid = verifyDoc.RootElement.GetProperty("data").GetProperty("is_valid").GetBoolean();
                
                if (!isValid)
                {
                    return BadRequest("Facebook token validation failed.");
                }
                
                // Get user info from Facebook
                var userInfoUrl = $"https://graph.facebook.com/me?fields=id,name,email,picture&access_token={request.Token}";
                var userInfoResponse = await client.GetAsync(userInfoUrl);
                
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to get user information from Facebook.");
                }
                
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                using var userDoc = JsonDocument.Parse(userInfoContent);
                
                var name = userDoc.RootElement.GetProperty("name").GetString();
                var email = userDoc.RootElement.TryGetProperty("email", out var emailElement) 
                    ? emailElement.GetString() 
                    : $"{userDoc.RootElement.GetProperty("id").GetString()}@facebook.com"; // Fallback if email not provided
                
                var pictureUrl = "";
                if (userDoc.RootElement.TryGetProperty("picture", out var pictureElement) && 
                    pictureElement.TryGetProperty("data", out var dataElement) && 
                    dataElement.TryGetProperty("url", out var urlElement))
                {
                    pictureUrl = urlElement.GetString();
                }
                
                // Check if user already exists in database
                var existingUser = await _customerService.GetCustomerByEmailAsync(email);
                
                if (existingUser == null)
                {
                    // Create a new account if it doesn't exist
                    string randomPassword = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());
                    
                    var newUser = new Customer
                    {
                        Email = email,
                        UserName = email.Split('@')[0], // Use part of email as username
                        Password = randomPassword,
                        FullName = name,
                        PictureUrl = pictureUrl,
                    };

                    var createdUser = await _customerService.CreateGoogleUserAsync(newUser);
                    existingUser = await _customerService.GetCustomerByEmailAsync(email);
                }

                // Generate JWT Token
                string role = "Customer";
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
                return BadRequest($"Error processing Facebook login: {ex.Message}");
            }
        }

        #endregion
    }
}
