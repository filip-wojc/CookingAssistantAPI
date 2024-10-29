namespace CookingAssistantAPI.Services.IngredientServices
{
    public interface IIngredientService
    {
        Task<List<string>> GetAllIngredientsListAsync();
    }
}
