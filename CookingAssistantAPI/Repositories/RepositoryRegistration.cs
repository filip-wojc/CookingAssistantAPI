using CookingAssistantAPI.Repositories.Recipes;
using CookingAssistantAPI.Repositories.Users;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Repositories
{
    public class RepositoryRegistration : IRegistrationResource
    {
        public void Register(IServiceCollection repositories) 
        {
            repositories.AddScoped<IRepositoryRecipe, RepositoryRecipe>();
            repositories.AddScoped<IRepositoryUser, RepositoryUser>();
        }
    }
}
