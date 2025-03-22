using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly MyDbContext _context;

        public ResetPasswordRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task CreateResetRequestAsync(ResetPassword request)
        {
            var existingRequest = await _context.ResetPasswords
                .FirstOrDefaultAsync(r => r.CustomerId == request.CustomerId);

            if (existingRequest != null)
            {
                existingRequest.Token = request.Token;
                existingRequest.ExpiryTime = request.ExpiryTime;
            }
            else
            {
                await _context.ResetPasswords.AddAsync(request);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ResetPassword?> GetValidResetRequestAsync(string resetPin)
        {
            return await _context.ResetPasswords
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.ResetPin == resetPin && r.ExpiryTime > DateTime.UtcNow);
        }

        public async Task RemoveResetRequestAsync(ResetPassword request)
        {
            _context.ResetPasswords.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}
