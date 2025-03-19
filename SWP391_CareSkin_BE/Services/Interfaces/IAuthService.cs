using SWP391_CareSkin_BE.DTOs.Requests.Customer;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IAuthService
    {
        void RequestPasswordReset(ForgotPasswordRequestDTO request);
        bool VerifyResetPin(VerifyResetPinDTO request);
        void ResetPassword(ResetPasswordDTO request);
    }
}
