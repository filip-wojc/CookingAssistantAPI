namespace CookingAssistantAPI.Database.Models
{
    public class Nutrient
    {
        public int Id { get; set; }
        public string? NutrientName { get; set; }
        // many to many relationship table reference
        public virtual ICollection<RecipeNutrient> RecipeNutrients { get; set; } = new List<RecipeNutrient>();
    }
}
