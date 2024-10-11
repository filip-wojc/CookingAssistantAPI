using CookingAssistantAPI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CookingAssistantAPI.Tools
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            (int statusCode, string message) = exception switch
            {
                NotFoundException ex => (ex.Code, ex.Message),
                _ => default
            };

            if (statusCode == default)
            {
                return false;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(message);

            return true;
        }
    }
}
