using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.DTOs.Requests.Customer;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly MyDbContext _context;

        public ResetPasswordRepository(MyDbContext context)
        {
            _context = context;
        }
        public void CreateResetRequest(ResetPassword request)
        {
            var existingRequest = _context.ResetPasswords
                .FirstOrDefault(r => r.CustomerId == request.CustomerId);

            if (existingRequest != null)
            {

                existingRequest.Token = request.Token;
                existingRequest.ExpiryTime = request.ExpiryTime;
            }
            else
            {
                _context.ResetPasswords.Add(request);
            }

            _context.SaveChanges();
        }


        public ResetPassword? GetValidResetRequest(string email, string resetPin)
        {
            return _context.ResetPasswords
                .Include(r => r.Customer)
                .FirstOrDefault(r => r.Customer.Email == email && r.ResetPin == resetPin && r.ExpiryTime > DateTime.UtcNow);
        }

        public void RemoveResetRequest(ResetPassword request)
        {
            _context.ResetPasswords.Remove(request);
            _context.SaveChanges();
        }
    }
}
