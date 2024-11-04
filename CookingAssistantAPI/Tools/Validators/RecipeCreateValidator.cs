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

            RuleFor(r => r.ImageData)
                .NotNull().WithMessage("Image data is required")
                .NotEmpty().WithMessage("Image data cannot be empty");

        }

        private bool IsValidValues(IEnumerable<string?> values)
        {
            // Znaki specjalne do zablokowania
            string[] specialChars = { "\\", " ", };
            var nullPattern = @"\bnull\b|\bNULL\b"; // Zablokuje samodzielne wystąpienia "null" i "NULL"

            foreach (var value in values)
            {
                // Sprawdź, czy wartość zawiera którykolwiek ze specjalnych znaków
                if (specialChars.Any(value.Contains))
                {
                    return false;
                }

                // Sprawdź, czy wartość zawiera słowo "null" jako osobne słowo
                if (System.Text.RegularExpressions.Regex.IsMatch(value, nullPattern))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidValue(string value)
        {
            // Znaki specjalne do zablokowania
            string[] specialChars = { "\\" }; // Dodaj inne znaki specjalne, które chcesz zablokować
            var nullPattern = @"\bnull\b|\bNULL\b"; // Zablokuje samodzielne wystąpienia "null" i "NULL"

            // Sprawdź, czy wartość zawiera którykolwiek ze specjalnych znaków
            if (specialChars.Any(value.Contains))
            {
                return false;
            }

            // Sprawdź, czy wartość zawiera słowo "null" jako osobne słowo
            if (System.Text.RegularExpressions.Regex.IsMatch(value, nullPattern))
            {
                return false;
            }

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
