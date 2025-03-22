using SWP391_CareSkin_BE.DTOs.Requests.Customer;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Repositories.Interfaces
{
    public interface IResetPasswordRepository
    {
        Task CreateResetRequestAsync(ResetPassword request);
        Task<ResetPassword?> GetValidResetRequestAsync(string resetPin);
        Task RemoveResetRequestAsync(ResetPassword request);
    }
}
