using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services
{
    public class ServicesRegistration : IRegistrationResource
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IRecipeService, RecipeService>();
        }
    }
}
