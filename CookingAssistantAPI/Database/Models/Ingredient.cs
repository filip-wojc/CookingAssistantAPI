namespace CookingAssistantAPI.Database.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string? IngredientName { get; set; }
        public virtual ICollection<Recipe>? Recipes { get; set; } = new List<Recipe>();
    }
}
