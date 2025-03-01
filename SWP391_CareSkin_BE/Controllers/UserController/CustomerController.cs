using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IFirebaseService _firebaseService;

        public CustomerController(ICustomerService customerService, IFirebaseService firebaseService)
        {
            _customerService = customerService;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound("Customer does not exist!!!");
            return Ok(customer);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterCustomerDTO request)
        {
            try
            {
                var customer = await _customerService.RegisterCustomerAsync(request);
                return Ok(new { message = "Register account successful", customer });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{customerId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateProfile(int customerId, [FromForm] UpdateProfileCustomerDTO request)
        {
            // 1. Nếu có file mới, upload file và lấy URL
            string newPictureUrl = null;
            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{request.PictureFile.FileName}";
                using var stream = request.PictureFile.OpenReadStream();

                newPictureUrl = await _firebaseService.UploadImageAsync(stream, fileName);
            }

            try
            {
                var updatedCustomer = await _customerService.UpdateProfileAsync(customerId, request, newPictureUrl);
                return Ok(new { message = "Update account successful", updatedCustomer });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomer(int customerId, [FromBody] string password)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(customerId, password);
                return Ok(new { message = "Delete account successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO adminDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginDTO loginDto = new LoginDTO
            {
                UserName = adminDTO.UserName,
                Password = adminDTO.Password
            };

            var authResult = await _customerService.Login(loginDto);

            return Ok(authResult);
        }

    }
}