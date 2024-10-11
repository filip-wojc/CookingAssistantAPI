namespace CookingAssistantAPI.Models
{
    public class Step
    {
        public int Id { get; set; }
        public Guid RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
