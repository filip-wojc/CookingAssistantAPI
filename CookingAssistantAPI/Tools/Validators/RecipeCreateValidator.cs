using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using FluentValidation;
using System.Reflection;

namespace CookingAssistantAPI.Tools.Validators
{
    public class RecipeCreateValidator : AbstractValidator<RecipeCreateDTO>
    {
        private readonly CookingDbContext _context;
        public RecipeCreateValidator(CookingDbContext context)
        {
            _context = context;

            RuleFor(r => r.Name).NotNull().NotEmpty().Must(IsValidValue).WithMessage("Don't use special characters");
            RuleFor(r => r.Description).NotNull().NotEmpty().Must(IsValidValue).WithMessage("Don't use special characters");
            RuleFor(r => r.Steps).Must(IsValidValues).WithMessage("Don't use special characters");
            RuleFor(r => r.Caloricity).NotNull().NotEmpty();
          

            RuleFor(r => r.IngredientNames)
                .NotNull().WithMessage("Ingredient names cannot be null")
                .NotEmpty().WithMessage("Ingredient names cannot be empty")
                .Must(IsValidValues).WithMessage("Don't use special characters");

            RuleFor(r => r.IngredientUnits)
                .NotNull().WithMessage("Ingredient units cannot be null")
                .Must(IsValidValues).WithMessage("Don't use special characters");

            RuleFor(r => r.IngredientQuantities)
                .NotNull().WithMessage("Ingredient quantities cannot be null")
                .Must(IsValidValues).WithMessage("Don't use special characters");

            RuleFor(r => r).Must(HaveSameListSize)
                .WithMessage("Ingredients or nutrients data is invalid");

            RuleFor(r => r.CategoryId).Must(CategoryExists)
                .WithMessage("This category does not exist");

            RuleFor(r => r.OccasionId).Must(OccasionExists)
                .WithMessage("This occasion does not exist");

            RuleFor(r => r.DifficultyId).Must(DifficultyExists)
                .WithMessage("This difficulty does not exist");

        }

        private bool IsValidValues(IEnumerable<string?> values)
        {
            if (values == null || !values.Any())
                return false; // Zablokuj puste listy

            string[] specialChars = { "\\", "\n", "\t" }; // Dodano więcej znaków
            var nullPattern = @"\bnull\b|\bNULL\b";

            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return false; // Zablokuj puste lub białe znaki

                if (specialChars.Any(c => value.Contains(c)))
                    return false; // Zablokuj znaki specjalne

                if (System.Text.RegularExpressions.Regex.IsMatch(value, nullPattern))
                    return false; // Zablokuj słowo "null"
            }

            return true;
        }

        private bool IsValidValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false; // Zablokuj puste wartości

            string[] specialChars = { "\\", "\n", "\t" };
            var nullPattern = @"\bnull\b|\bNULL\b";

            if (specialChars.Any(c => value.Contains(c)))
                return false; // Zablokuj znaki specjalne

            if (System.Text.RegularExpressions.Regex.IsMatch(value, nullPattern))
                return false; // Zablokuj słowo "null"

            return true;
        }



        private bool HaveSameListSize(RecipeCreateDTO dto)
        {
            // Handle potential nulls safely
            if (dto.IngredientNames == null || dto.IngredientUnits == null || dto.IngredientQuantities == null)
            {
                return false;
            }

            var ing = dto.IngredientNames.Count();

            bool ingredientsSameSize = ing == dto.IngredientUnits.Count() && ing == dto.IngredientQuantities.Count();

            return ingredientsSameSize;
        }

        private bool CategoryExists(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId) != null;
        }

        private bool DifficultyExists(int difficultyId)
        {
            return _context.Difficulties.FirstOrDefault(d => d.Id == difficultyId) != null;
        }

        private bool OccasionExists(int occasionId)
        {
            return _context.Occasions.FirstOrDefault(o => o.Id == occasionId) != null;
        }
    }
}
