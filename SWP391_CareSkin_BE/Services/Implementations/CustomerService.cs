using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IFirebaseService _firebaseService;
        private readonly MyDbContext _context;

        public CustomerService(ICustomerRepository customerRepository, IFirebaseService firebaseService, MyDbContext context)
        {
            _customerRepository = customerRepository;
            _firebaseService = firebaseService;
            _context = context;
        }

        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return customers.Select(CustomerMapper.ToCustomerResponseDTO).ToList();
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            return customer != null ? CustomerMapper.ToCustomerResponseDTO(customer) : null;
        }

        public async Task<CustomerDTO> RegisterCustomerAsync(RegisterCustomerDTO request)
        {
            var existingCustomer = await _customerRepository.GetCustomerByEmailOrUsernameAsync(request.Email, request.UserName);
            if (existingCustomer != null && existingCustomer.IsActive)
            {
                throw new ArgumentException("Email or username already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newCustomer = CustomerMapper.ToCustomer(request, hashedPassword);
            await _customerRepository.AddCustomerAsync(newCustomer);

            return CustomerMapper.ToCustomerResponseDTO(newCustomer);
        }

        public async Task<CustomerDTO> UpdateProfileAsync(int customerId, UpdateProfileCustomerDTO request, string pictureUrl)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                throw new ArgumentException("Khách hàng không tồn tại.");
            }

            CustomerMapper.UpdateCustomer(customer, request, pictureUrl);
            await _customerRepository.UpdateCustomerAsync(customer);

            return CustomerMapper.ToCustomerResponseDTO(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId, string password)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                throw new ArgumentException("Khách hàng không tồn tại.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, customer.Password))
            {
                throw new ArgumentException("Mật khẩu không đúng.");
            }

            // Implement soft delete by setting IsActive to false instead of removing from database
            customer.IsActive = false;
            await _customerRepository.UpdateCustomerAsync(customer);
            return true;
        }

        public async Task<CustomerDTO> Login(LoginDTO loginDto)
        {
            var authResult = await _customerRepository.LoginCustomer(loginDto);
            return CustomerMapper.ToCustomerResponseDTO(authResult);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _customerRepository.GetCustomerByEmailAsync(email);
        }

        public async Task<CustomerDTO> CreateGoogleUserAsync(Customer customer)
        {
            await _customerRepository.AddCustomerAsync(customer);
            return CustomerMapper.ToCustomerResponseDTO(customer);
        }

       
    }
}
