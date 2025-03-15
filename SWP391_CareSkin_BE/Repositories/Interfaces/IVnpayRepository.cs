using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IVnpayRepository
    {
        Task<VnpayTransactions> AddTransactionAsync(VnpayTransactions transaction);
        Task<VnpayTransactions> GetByOrderIdAsync(int orderId);
        Task UpdateTransactionAsync(VnpayTransactions transaction);
    }
}
