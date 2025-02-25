using System.IdentityModel.Tokens.Jwt;

namespace SWP391_CareSkin_BE.MiddleWare
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "role");
                string role = roleClaim.Value;
                if (role == "Admin")
                {
                    context.Items["Role"] = "Admin";
                }
                else if (role == "Staff")
                {
                    context.Items["Role"] = "Staff";
                }
                else if (role == "Customer")
                {
                    context.Items["Role"] = "Customer";
                }
            }

            await _next(context);
        }
    }
}
