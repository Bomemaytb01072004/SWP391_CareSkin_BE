using SWP391_CareSkin_BE.DTOs.Requests;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IMomoService
    {
        string CreatePayment(MomoRequest request);
    }
}
