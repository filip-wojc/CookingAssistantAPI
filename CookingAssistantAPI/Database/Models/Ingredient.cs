namespace CookingAssistantAPI.Database.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string? IngredientName { get; set; }
        // many to many relationship table reference
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
