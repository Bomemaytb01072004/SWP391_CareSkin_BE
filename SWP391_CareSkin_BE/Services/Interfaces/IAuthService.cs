using SWP391_CareSkin_BE.DTOs.Requests.Customer;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAuthService
    {
        Task RequestPasswordReset(ForgotPasswordRequestDTO request);
        Task<bool> VerifyResetPin(VerifyResetPinDTO request);
        Task ResetPassword(ResetPasswordDTO request);
    }
}
