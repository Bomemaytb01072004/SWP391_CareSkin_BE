using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IVnpayRepository
    {
        Task AddTransactionAsync(VnpayTransactions transaction);
        Task<VnpayTransactions> GetTransactionByOrderIdAsync(string orderId);
        Task UpdateTransactionAsync(VnpayTransactions transaction);
    }
}
