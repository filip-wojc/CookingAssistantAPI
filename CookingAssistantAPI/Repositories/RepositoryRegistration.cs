using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Repositories
{
    public class RepositoryRegistration : IRegistrationResource
    {
        public void Register(IServiceCollection repositories) 
        {
            repositories.AddScoped<IRepositoryRecipe, RepositoryRecipe>();
        }
    }
}
