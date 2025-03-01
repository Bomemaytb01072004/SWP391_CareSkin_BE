using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IMomoRepository
    {
        void SavePaymentTransaction(MomoTransaction transaction);
        MomoTransaction GetTransactionByOrderId(string orderId);
    }
}
