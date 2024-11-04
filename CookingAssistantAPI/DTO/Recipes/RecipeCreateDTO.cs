using CookingAssistantAPI.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace CookingAssistantAPI.DTO.Recipes
{
    public class RecipeCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageData { get; set; }
        public int Serves { get; set; }
        public string Difficulty { get; set; }
        public int? TimeInMinutes { get; set; }
        public int CategoryId { get; set; }
        public string Occasion {  get; set; }
        public int Caloricity {  get; set; }
        public List<string>? IngredientNames { get; set; }
        public List<string>? IngredientQuantities { get; set; }
        public List<string>? IngredientUnits { get; set; }
        public List<string>? Steps { get; set; }

    }
}
