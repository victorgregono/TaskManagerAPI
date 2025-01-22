using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskManagerAPI.Middlewares
{
    public class UserRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public UserRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //if (context.Request.Headers.TryGetValue("X-User-Role", out var role))
            //{
            //    var claims = new List<Claim> { new Claim(ClaimTypes.Role, role) };
            //    var identity = new ClaimsIdentity(claims, "Custom");
            //    context.User = new ClaimsPrincipal(identity);
            //}
            //else
            //{
            //    // Adicionar uma reivindicação de função padrão se o cabeçalho não estiver presente
            //    var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Guest") };
            //    var identity = new ClaimsIdentity(claims, "Custom");
            //    context.User = new ClaimsPrincipal(identity);
            //}


            var role = context.Request.Headers.TryGetValue("X-User-Role", out var userRole) ? userRole.ToString() : "Guest";
            var claims = new List<Claim> { new Claim(ClaimTypes.Role, role) };
            var identity = new ClaimsIdentity(claims, "Custom");
            context.User = new ClaimsPrincipal(identity);



            await _next(context);
        }
    }

    public static class UserRoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserRoleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserRoleMiddleware>();
        }
    }
}
