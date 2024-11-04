using CookingAssistantAPI.DTO.RecipeIngredients;
using CookingAssistantAPI.DTO.Steps;

namespace CookingAssistantAPI.DTO.Recipes
{
    public class RecipeGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public float Ratings { get; set; }
        public int TimeInMinutes { get; set; }
        public int Serves { get; set; }
        public string? Difficulty { get; set; }
        public int VoteCount { get; set; }
        public string CategoryName { get; set; }
        public string Occasion {  get; set; }
        public int Caloricity { get; set; }
        public List<RecipeIngredientGetDTO>? Ingredients { get; set; }
        public List<StepGetDTO>? Steps { get; set; }

    }
}
