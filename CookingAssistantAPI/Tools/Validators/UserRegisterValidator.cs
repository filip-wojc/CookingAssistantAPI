using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Database;
using FluentValidation;

namespace CookingAssistantAPI.Tools.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
    {
        private readonly CookingDbContext _context;
        public UserRegisterValidator(CookingDbContext context)
        {
            _context = context;

            RuleFor(u => u.Email).NotEmpty().EmailAddress().Must(IsEmailUnique).WithMessage("Email already used").Must(IsValidValue);
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(5).MaximumLength(24).Must(IsUserNameUnique).WithMessage("Username is already taken").Must(IsValidValue);
            RuleFor(u => u.Password).NotEmpty()
                .MinimumLength(5).MaximumLength(30)
                .Matches(@"[A-Z]").WithMessage("Hasło musi mieć co najmniej jedną wielką literę")
                .Matches(@"\d").WithMessage("Hasło musi zawierac co najmniej jedną cyfrę");
        }
        private bool IsValidValue(string value)
        {
            // Znaki specjalne do zablokowania
            string[] specialChars = { "\\"," " }; // Dodaj inne znaki specjalne, które chcesz zablokować
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

        bool IsEmailUnique(string email)
        { 
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                return false;
            }
            return true;
        }

        bool IsUserNameUnique(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                return false;
            }
            return true;


        }
    }
}
