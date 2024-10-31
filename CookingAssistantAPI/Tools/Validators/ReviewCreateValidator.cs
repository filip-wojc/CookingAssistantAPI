using CookingAssistantAPI.DTO.Reviews;
using FluentValidation;

namespace CookingAssistantAPI.Tools.Validators
{
    public class ReviewCreateValidator : AbstractValidator<ReviewCreateDTO>
    {
        public ReviewCreateValidator()
        {
            RuleFor(r => r.Value).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
            RuleFor(r => r.Description).Must(IsValidValue);
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
    }
}
