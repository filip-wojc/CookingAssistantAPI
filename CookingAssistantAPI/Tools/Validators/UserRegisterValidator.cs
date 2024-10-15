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

            RuleFor(u => u.Email).NotEmpty().EmailAddress().Must(IsEmailUnique).WithMessage("Email already used");
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(5).MaximumLength(24).Must(IsUserNameUnique).WithMessage("Username is already taken");
            RuleFor(u => u.Password).NotEmpty()
                .MinimumLength(5).MaximumLength(30)
                .Matches(@"[A-Z]").WithMessage("Hasło musi mieć co najmniej jedną wielką literę")
                .Matches(@"\d").WithMessage("Hasło musi zawierac co najmniej jedną cyfrę");
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
