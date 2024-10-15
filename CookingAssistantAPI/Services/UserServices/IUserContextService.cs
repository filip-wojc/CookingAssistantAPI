using System.Security.Claims;

namespace CookingAssistantAPI.Services.UserServices
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
        string UserRole { get; }
    }
}
