namespace CookingAssistantAPI.Database.Models
{
    public class Time
    {
        public int Id { get; set; }
        public virtual Recipe? Recipe { get; set; }
        public int RecipeId { get; set; }
        public string? Type { get; set; }
        public string? TimeValue { get; set; }
    }
}
