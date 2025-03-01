using System;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Implementations
{
    public class MomoRepository
    {
        private readonly MyDbContext _context;

        public MomoRepository(MyDbContext context)
        {
            _context = context;
        }

        public void SavePaymentTransaction(MomoTransaction transaction)
        {
            _context.MomoTransactions.Add(transaction);
            _context.SaveChanges();
        }

        public MomoTransaction GetTransactionByOrderId(string orderId)
        {
            return _context.MomoTransactions.FirstOrDefault(t => t.OrderId == orderId);
        }
    }
}
