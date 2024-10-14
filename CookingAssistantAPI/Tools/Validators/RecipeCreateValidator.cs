using CookingAssistantAPI.Database;
using CookingAssistantAPI.DTO;
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
            RuleFor(r => r.CategoryId).Must(CategoryExists);
            RuleFor(r => r.ImageData).NotEmpty();
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

        /*
         * TODO : Check for null fields
         * 
         * 
         */



    }
}
