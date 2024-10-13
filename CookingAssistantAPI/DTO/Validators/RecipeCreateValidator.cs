using FluentValidation;

namespace CookingAssistantAPI.DTO.Validators
{
    public class RecipeCreateValidator : AbstractValidator<RecipeCreateDTO>
    {
        public RecipeCreateValidator()
        {
            RuleFor(r => r).Must(HaveSameListSize)
                .WithMessage("Ingredients or nutrients invalid data");
        }

        private bool HaveSameListSize(RecipeCreateDTO dto)
        {
            var ing = dto.IngredientNames.Count();
            var nuts = dto.NutrientNames.Count();

            bool IngredientsSameSize = ing == dto.IngredientUnits.Count() && ing == dto.IngredientQuantities.Count();
            bool NutrientsSameSize = nuts == dto.NutrientUnits.Count() && nuts == dto.NutrientQuantities.Count();

            return IngredientsSameSize && NutrientsSameSize;
        }
    }
}
