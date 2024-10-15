using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.DTO.RecipeIngredients
{
    public class RecipeIngredientGetDTO
    {
        public string? IngredientName { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
    }
}
