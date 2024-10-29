using CookingAssistantAPI.Repositories.Ingredients;

namespace CookingAssistantAPI.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly IRepositoryIngredient _repository;
        public IngredientService(IRepositoryIngredient repository)
        {
            _repository = repository;
        }
        public async Task<List<string>> GetAllIngredientsListAsync()
        {
            return await _repository.GetAllIngredientsListAsync();
        }

    }
}
