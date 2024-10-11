namespace CookingAssistantAPI.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public Guid RecipeId { get; set; }
        public string IngredientName { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
