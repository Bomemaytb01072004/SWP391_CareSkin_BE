using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SWP391_CareSkin_BE.Services
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Result is ForbidResult)
            {
                context.Result = new ObjectResult(new { message = "You are not authorized to access this resource." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
