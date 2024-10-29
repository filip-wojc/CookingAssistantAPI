namespace CookingAssistantAPI.Services.NutrientServices
{
    public interface INutrientService
    {
        Task<List<string>> GetAllNutrientsListAsync();
    }
}
