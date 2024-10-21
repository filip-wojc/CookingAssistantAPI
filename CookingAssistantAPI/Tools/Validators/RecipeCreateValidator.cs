using CookingAssistantAPI.Database;
using CookingAssistantAPI.DTO.Recipes;
using FluentValidation;

namespace CookingAssistantAPI.Tools.Validators
{
    public class RecipeCreateValidator : AbstractValidator<RecipeCreateDTO>
    {
        private readonly CookingDbContext _context;
        public RecipeCreateValidator(CookingDbContext context)
        {
            _context = context;

            RuleFor(r => r).Must(HaveSameListSize)
                .WithMessage("Ingredients or nutrients invalid data");
            RuleFor(r => r.CategoryId).Must(CategoryExists).WithMessage("This categorry does not exist");
            RuleFor(r => r.ImageData).NotEmpty();
            RuleFor(r => r.Difficulty).Must(CorrectDifficulty).WithMessage("Difficulty must be: easy, medium or hard");
        }

        private bool HaveSameListSize(RecipeCreateDTO dto)
        {
            var ing = dto.IngredientNames.Count();
            var nuts = dto.NutrientNames.Count();

            bool IngredientsSameSize = ing == dto.IngredientUnits.Count() && ing == dto.IngredientQuantities.Count();
            bool NutrientsSameSize = nuts == dto.NutrientUnits.Count() && nuts == dto.NutrientQuantities.Count();

            return IngredientsSameSize && NutrientsSameSize;
        }

        private bool CategoryExists(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId) != null;

        }

        private bool CorrectDifficulty(string difficulty)
        {
            var correctDifficulties = new[] { "easy", "medium", "hard" };

            if (correctDifficulties.Contains(difficulty.ToLower()))
            {
                return true;
            }
            return false;
        }

        /*
         * TODO : Check for null fields
         * 
         * 
         */



    }
}
