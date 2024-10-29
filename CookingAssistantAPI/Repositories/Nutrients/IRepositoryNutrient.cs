namespace CookingAssistantAPI.Repositories.Nutrients
{
    public interface IRepositoryNutrient
    {
        Task<List<string>> GetAllNutrientsListAsync();
    }
}
