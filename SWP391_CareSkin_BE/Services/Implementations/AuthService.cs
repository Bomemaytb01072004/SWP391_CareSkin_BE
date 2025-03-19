using SWP391_CareSkin_BE.DTOs.Requests.Customer;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using SWP391_CareSkin_BE.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System;
using SWP391_CareSkin_BE.Data;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IResetPasswordRepository _passwordResetRepository;
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public AuthService(ICustomerRepository customerRepository, IResetPasswordRepository passwordResetRepository, IConfiguration configuration, MyDbContext context)
        {
            _customerRepository = customerRepository;
            _passwordResetRepository = passwordResetRepository;
            _configuration = configuration;
            _context = context;
        }

        public void RequestPasswordReset(ForgotPasswordRequestDTO request)
        {
            var customer = _customerRepository.GetCustomerByEmailAsync(request.Email).Result;
            if (customer == null)
            {
                throw new Exception("Email không tồn tại.");
            }

            var pin = new Random().Next(100000, 999999).ToString();
            var expiryTime = DateTime.UtcNow.AddMinutes(15);

            var resetRequest = new ResetPassword
            {
                CustomerId = customer.CustomerId,
                ResetPin = pin,
                ExpiryTime = expiryTime
            };

            _passwordResetRepository.CreateResetRequest(resetRequest);

            SendEmail(customer.Email, "Mã PIN đặt lại mật khẩu", $"Mã PIN của bạn là: {pin}");
        }

        public bool VerifyResetPin(VerifyResetPinDTO request)
        {
            var resetRequest = _passwordResetRepository.GetValidResetRequest(request.Email, request.ResetPin);
            return resetRequest != null;
        }

        public void ResetPassword(ResetPasswordDTO request)
        {
            var resetRequest = _passwordResetRepository.GetValidResetRequest(request.Email, request.ResetPin);
            if (resetRequest == null)
            {
                throw new Exception("Mã PIN không hợp lệ hoặc đã hết hạn.");
            }

            var customer = _customerRepository.GetCustomerByEmailAsync(request.Email).Result;
            if (customer == null)
            {
                throw new Exception("Email không tồn tại.");
            }

            customer.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _customerRepository.UpdateCustomerAsync(customer);

            _passwordResetRepository.RemoveResetRequest(resetRequest);
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            string smtpServer = _configuration["EmailSettings:SmtpServer"];
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            string senderEmail = _configuration["EmailSettings:SenderEmail"];
            string senderPassword = _configuration["EmailSettings:SenderPassword"];
            bool enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = enableSsl;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                client.Send(mailMessage);
            }
        }

    }
}
