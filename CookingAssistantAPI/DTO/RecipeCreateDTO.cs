using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.DTO
{
    public class RecipeCreateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageData { get; set; } 
        public int Serves { get; set; }
        public string? Difficulty { get; set; }
        public int? TimeInMinutes {  get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<string>? IngredientNames { get; set; }
        public virtual ICollection<string>? Steps { get; set; }
        public virtual ICollection<NutrientCreateDTO>? Nutrients { get; set; }
        
    }
}
