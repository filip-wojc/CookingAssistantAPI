namespace CookingAssistantAPI.Models
{
    public class Nutrient
    {
        public int Id { get; set; }
        public Guid RecipeId { get; set; }
        public string NutrientName { get; set; }
        public string Value { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
