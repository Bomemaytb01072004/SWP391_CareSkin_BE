using System;
using Google;
using Microsoft.EntityFrameworkCore;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Repositories.Interfaces;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class VnpayRepository : IVnpayRepository
    {
        private readonly MyDbContext _context;

        public VnpayRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task AddTransactionAsync(VnpayTransactions transaction)
        {
            await _context.VnpayTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<VnpayTransactions> GetTransactionByOrderIdAsync(string orderId)
        {
            return await _context.VnpayTransactions.FirstOrDefaultAsync(t => t.OrderId == orderId);
        }

        public async Task UpdateTransactionAsync(VnpayTransactions transaction)
        {
            _context.VnpayTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
