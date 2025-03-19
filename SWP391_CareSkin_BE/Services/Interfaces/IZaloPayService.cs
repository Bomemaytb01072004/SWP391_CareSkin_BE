using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IZaloPayService
    {
        Task<Dictionary<string, object>> CreateOrderAsync(ZaloPayOrder order);
        Task<Dictionary<string, object>> QueryOrderAsync(string appTransId);
        string ComputeHmac(string data, string key);
    }
}
