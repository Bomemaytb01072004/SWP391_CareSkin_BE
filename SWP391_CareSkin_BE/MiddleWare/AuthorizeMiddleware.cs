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
                var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;

                if (roleClaim == "Admin")
                {
                    context.Items["Role"] = "Admin";
                }
                else if (roleClaim == "Staff")
                {
                    context.Items["Role"] = "Staff";
                }
                else if (roleClaim == "Customer")
                {
                    context.Items["Role"] = "Customer";
                }
            }

            await _next(context);
        }
    }
}
