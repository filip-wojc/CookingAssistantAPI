using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.DTO.RecipeNutrients
{
    public class RecipeNutrientGetDTO
    {
        public string? NutrientName { get; set; }
        public string? Quantity { get; set; }
        public string? Unit { get; set; }
    }
}
