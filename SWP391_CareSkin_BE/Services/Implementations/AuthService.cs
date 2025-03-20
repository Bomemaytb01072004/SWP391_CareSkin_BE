﻿using SWP391_CareSkin_BE.DTOs.Requests.Customer;
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
            var expiryTime = DateTime.UtcNow.AddMinutes(15);

            var resetRequest = new ResetPassword
            {
                CustomerId = customer.CustomerId,
                ResetPin = pin,
                ExpiryTime = expiryTime
            };

            await _passwordResetRepository.CreateResetRequestAsync(resetRequest);
            await _emailService.SendPINForResetPassword(customer.Email, customer.FullName, resetRequest.ResetPin);
        }

        public async Task<bool> VerifyResetPin(VerifyResetPinDTO request)
        {
            var resetRequest = await _passwordResetRepository.GetValidResetRequestAsync(request.Email, request.ResetPin);
            return resetRequest != null;
        }

        public async Task ResetPassword(ResetPasswordDTO request)
        {
            var resetRequest = await _passwordResetRepository.GetValidResetRequestAsync(request.Email, request.ResetPin);
            if (resetRequest == null) throw new Exception("Invalid or expired PIN.");

            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email);
            if (customer == null) throw new Exception("Email does not exist.");

            customer.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _customerRepository.UpdateCustomerAsync(customer);

            await _passwordResetRepository.RemoveResetRequestAsync(resetRequest);
        }

        
    }
}
