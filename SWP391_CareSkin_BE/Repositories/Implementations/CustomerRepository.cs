using System;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.Helpers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MyDbContext _context;
        private readonly JwtHelper _jwtHelper;
        public CustomerRepository(MyDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> GetCustomerByEmailOrUsernameAsync(string email, string username)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email || c.UserName == username);
        }

        public async Task<LoginResult> LoginCustomer(LoginDTO request)
        {
            var admin = await _context.Customers.FirstOrDefaultAsync(a => a.UserName == request.UserName);

            if (admin == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid username",
                };
            }

            if (!Validate.VerifyPassword(admin.Password, request.Password))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid password.",
                };
            }

            string role = "User";
            var token = _jwtHelper.GenerateToken(request.UserName, role);
            return new LoginResult
            {
                Success = true,
                Message = "User is logged",
                Data = token,
            };
        }
    }
}
