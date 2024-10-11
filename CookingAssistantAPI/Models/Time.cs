namespace CookingAssistantAPI.Models
{
    public class Time
    {
        public int Id { get; set; }
        public Guid RecipeId { get; set; }
        public string Type { get; set; }
        public string TimeValue { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
