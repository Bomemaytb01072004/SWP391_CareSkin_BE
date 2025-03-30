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
            try
            {
                var existingRequest = await _context.ResetPasswords
                    .FirstOrDefaultAsync(r => r.CustomerId == request.CustomerId);

                if (existingRequest != null)
                {
                    existingRequest.ResetPin = request.ResetPin;
                    existingRequest.Token = request.Token;
                    existingRequest.ExpiryTime = request.ExpiryTime;
                    Console.WriteLine($"Updating existing reset request for customer {request.CustomerId}");
                }
                else
                {
                    await _context.ResetPasswords.AddAsync(request);
                    Console.WriteLine($"Creating new reset request for customer {request.CustomerId}");
                }

                var saveResult = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync result: {saveResult} records affected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateResetRequestAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ResetPassword?> GetValidResetRequestAsync(string resetPin)
        {
            return await _context.ResetPasswords
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.ResetPin == resetPin && r.ExpiryTime > DateTime.Now);
        }

        public async Task RemoveResetRequestAsync(ResetPassword request)
        {
            _context.ResetPasswords.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}
