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

        public async Task<VnpayTransactions> AddTransactionAsync(VnpayTransactions transaction)
        {
            _context.VnpayTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<VnpayTransactions> GetByOrderIdAsync(int orderId)
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
