namespace CookingAssistantAPI.Database.Models
{
    public class RecipeNutrient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int NutrientId { get; set; }
        public Nutrient Nutrient { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
    }
}
