using System.Security.Claims;
using Microsoft.AspNetCore.Http;
//using System.Security.Claims;
namespace PayFlow.API.Middleware;

public class FirstLoginMiddleware
{
    private readonly RequestDelegate _next;

    public FirstLoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Allow login and change-password endpoints through always
        var path = context.Request.Path.Value?.ToLower();
        if (path != null && (
            path.Contains("/auth/login") ||
            path.Contains("/auth/change-password")))
        {
            await _next(context);
            return;
        }

        // Check if user is authenticated
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // If IsFirstLogin claim is true, block everything else
            var isFirstLogin = context.User.FindFirst("IsFirstLogin")?.Value;
            if (isFirstLogin == "true")
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Please change your password before continuing."
                });
                return;
            }
        }

        await _next(context);
    }
}
