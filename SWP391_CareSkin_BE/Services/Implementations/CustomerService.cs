using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Requests.Customer;
using SWP391_CareSkin_BE.DTOS;
using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Text.RegularExpressions;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IFirebaseService _firebaseService;

        public CustomerService(ICustomerRepository customerRepository, IFirebaseService firebaseService)
        {
            _customerRepository = customerRepository;
            _firebaseService = firebaseService;
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
            if (existingCustomer != null)
            {
                throw new ArgumentException("Email hoặc username đã tồn tại.");
            }

            // Kiểm tra email
            if (!Regex.IsMatch(request.Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                throw new ArgumentException("Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com");
            }

            // Kiểm tra số điện thoại
            if (!string.IsNullOrEmpty(request.Phone) && !Regex.IsMatch(request.Phone, @"^0\d{10}$"))
            {
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và có 11 chữ số.");
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

            // Kiểm tra email
            if (!string.IsNullOrEmpty(request.Email) && !Regex.IsMatch(request.Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                throw new ArgumentException("Email phải có định dạng hợp lệ và kết thúc bằng @gmail.com");
            }

            // Kiểm tra số điện thoại
            if (!string.IsNullOrEmpty(request.Phone) && !Regex.IsMatch(request.Phone, @"^0\d{10}$"))
            {
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và có 11 chữ số.");
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

            await _customerRepository.DeleteCustomerAsync(customer);
            return true;
        }

        public async Task<LoginResult> Login(LoginDTO loginDto)
        {
            var authResult = await _customerRepository.LoginCustomer(loginDto);
            return authResult;
        }

        public async Task<bool> ChangePasswordAsync(int customerId, ChangePasswordDTO request)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                throw new ArgumentException("Khách hàng không tồn tại.");
            }

            // Kiểm tra mật khẩu hiện tại
            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, customer.Password))
            {
                throw new ArgumentException("Mật khẩu hiện tại không đúng.");
            }

            // Cập nhật mật khẩu mới
            customer.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _customerRepository.UpdateCustomerAsync(customer);
            return true;
        }

        

       
    }
}
