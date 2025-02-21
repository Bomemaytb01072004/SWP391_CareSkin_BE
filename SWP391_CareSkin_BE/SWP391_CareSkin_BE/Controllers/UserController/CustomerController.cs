using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services;

namespace SWP391_CareSkin_BE.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CustomerController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email))

            {
                return BadRequest(new { message = "Username, password and email cannot be empty!" });
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Password is incorrect!" });
            }

            var existingUser = await _context.Customers
                .FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "UserName or Email already exists!" });
            }

            string hashedPassword = Validate.HashPassword(request.Password);
            var newUser = CustomerMapper.ToCustomer(request,hashedPassword);
            await _context.Customers.AddAsync(newUser);
            await _context.SaveChangesAsync(); 
            
            return Ok(new { message = "Register successfully!", userId = newUser.CustomerId });

        }


        [HttpPut("Update-Profile/{customerId}")]
        public async Task<IActionResult> UpdateProfile(int customerId, [FromBody] UpdateProfileCustomerDTO updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound(new { message = "UserName does not exsit!!" });
            }
            CustomerMapper.UpdateCustomer(customer, updatedCustomer);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Update successfully!" });
        }



        [HttpDelete("Delete-Account/{customerId}")]
        public async Task<IActionResult> DeleteAccount(int customerId, [FromBody] DeleteAccountCustomerDTO request)
        {
            // Tìm khách hàng theo ID
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound(new { message = "UserName does not exsit!" });
            }

            // Kiểm tra mật khẩu nhập vào với mật khẩu hash trong database
            if (!Validate.VerifyPassword(customer.Password, request.Password))
            {
                return BadRequest(new { message = "Password is incorrect!" });
            }

            // Xóa tài khoản
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "The account has been successfully deleted!" });
        }

        [HttpGet("GetById/{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound(new { message = "User does not exsit!" });
            }
            var customerResponse = CustomerMapper.ToCustomerResponseDTO(customer);
            return Ok(customerResponse);
        }

    }
}