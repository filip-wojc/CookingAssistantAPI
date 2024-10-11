namespace CookingAssistantAPI.Database.Models
{
    public class Nutrient
    {
        public int Id { get; set; }
        public string? NutrientName { get; set; }
        public string? Value { get; set; }
        public virtual ICollection<Recipe>? Recipes { get; set; } = new List<Recipe>();
    }
}
