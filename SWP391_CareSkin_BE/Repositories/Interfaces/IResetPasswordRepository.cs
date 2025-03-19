using SWP391_CareSkin_BE.DTOs.Requests.Customer;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IResetPasswordRepository
    {
        void CreateResetRequest(ResetPassword request);
        ResetPassword? GetValidResetRequest(string email, string resetPin);
        void RemoveResetRequest(ResetPassword request);
    }
}
