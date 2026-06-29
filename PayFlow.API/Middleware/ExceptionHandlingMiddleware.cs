using PayFlow.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
//using PayFlow.Domain.Exceptions;
//using PayFlow.Domain.Exceptions;
namespace PayFlow.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (DuplicateRecordException ex)
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (PayrollLockedException ex)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { message = "Something went wrong.", detail = ex.Message });
        }
    }
}
