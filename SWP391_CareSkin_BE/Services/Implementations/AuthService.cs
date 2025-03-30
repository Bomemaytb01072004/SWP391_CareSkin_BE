using SWP391_CareSkin_BE.DTOs.Requests.Customer;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IResetPasswordRepository _passwordResetRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(ICustomerRepository customerRepository, IResetPasswordRepository passwordResetRepository, IConfiguration configuration, IEmailService emailService)
        {
            _customerRepository = customerRepository;
            _passwordResetRepository = passwordResetRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task RequestPasswordReset(ForgotPasswordRequestDTO request)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
            if (customer == null) throw new Exception("Email does not exist.");

            var pin = new Random().Next(100000, 999999).ToString();
            // Use DateTime.Now instead of UtcNow for consistency with validation
            var expiryTime = DateTime.Now.AddMinutes(15);

            var resetRequest = new ResetPassword
            {
                CustomerId = customer.CustomerId,
                ResetPin = pin,
                ExpiryTime = expiryTime
            };

            try
            {
                await _passwordResetRepository.CreateResetRequestAsync(resetRequest);
                Console.WriteLine($"Reset PIN created for customer {customer.CustomerId}: {pin}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating reset PIN: {ex.Message}");
                throw new Exception($"Failed to create reset PIN: {ex.Message}", ex);
            }

            await _emailService.SendPINForResetPassword(customer.Email, customer.FullName, resetRequest.ResetPin);
        }

        public async Task<bool> VerifyResetPin(VerifyResetPinDTO request)
        {
            var resetRequest = await _passwordResetRepository.GetValidResetRequestAsync(request.ResetPin);
            if (resetRequest == null)
            {
                throw new Exception("Invalid or expired PIN.");
            }
            else
            {
                await _passwordResetRepository.RemoveResetRequestAsync(resetRequest);
            }
            return resetRequest != null;
        }

        public async Task ResetPassword(ResetPasswordDTO request)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
            if (customer == null) throw new Exception("Email does not exist.");

            customer.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _customerRepository.UpdateCustomerAsync(customer);
        }
    }
}
