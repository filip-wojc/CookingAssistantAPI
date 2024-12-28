using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CookingAssistantAPI.IntegrationTests
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipals = new ClaimsPrincipal();

            claimsPrincipals.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Role, "User")
                }));

            context.HttpContext.User = claimsPrincipals;

            await next();
        }
    }
}
