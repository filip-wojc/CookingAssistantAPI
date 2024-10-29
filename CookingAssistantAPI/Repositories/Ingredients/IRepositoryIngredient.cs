namespace CookingAssistantAPI.Repositories.Ingredients
{
    public interface IRepositoryIngredient
    {
        Task<List<string>> GetAllIngredientsListAsync();
    }
}
