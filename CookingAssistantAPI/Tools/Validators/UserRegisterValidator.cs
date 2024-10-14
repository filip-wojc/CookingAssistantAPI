using CookingAssistantAPI.DTO.Users;
using FluentValidation;

namespace CookingAssistantAPI.Tools.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(5).MaximumLength(24);
            RuleFor(u => u.Password).NotEmpty()
                .MinimumLength(5).MaximumLength(30)
                .Matches(@"[A-Z]").WithMessage("Hasło musi mieć co najmniej jedną wielką literę")
                .Matches(@"\d").WithMessage("Hasło musi zawierac co najmniej jedną cyfrę");
        }
    }
}
