using CookingAssistantAPI.Repositories.Nutrients;

namespace CookingAssistantAPI.Services.NutrientServices
{
    public class NutrientService : INutrientService
    {
        private readonly IRepositoryNutrient _repository;
        public NutrientService(IRepositoryNutrient repository)
        {
            _repository = repository;
        }
        public async Task<List<string>> GetAllNutrientsListAsync()
        {
            return await _repository.GetAllNutrientsListAsync();
        }

    }
}
