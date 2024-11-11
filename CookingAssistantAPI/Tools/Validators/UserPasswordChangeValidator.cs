using CookingAssistantAPI.DTO.Users;
using FluentValidation;

namespace CookingAssistantAPI.Tools.Validators
{
    public class UserPasswordChangeValidator : AbstractValidator<UserPasswordChangeDTO>
    {
        public UserPasswordChangeValidator()
        {
            RuleFor(u => u.NewPassword).NotEmpty()
               .MinimumLength(5).MaximumLength(30)
               .Matches(@"[A-Z]").WithMessage("Password must have at least one capital letter")
               .Matches(@"\d").WithMessage("Password must have at least one digit");

            RuleFor(u => u).Must(u => u.NewPassword == u.NewPasswordConfirm).WithMessage("Passwords are not equal");
        }
    }
}
