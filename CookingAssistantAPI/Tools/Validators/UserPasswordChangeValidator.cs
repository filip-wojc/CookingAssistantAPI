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
               .Matches(@"[A-Z]").WithMessage("Hasło musi mieć co najmniej jedną wielką literę")
               .Matches(@"\d").WithMessage("Hasło musi zawierac co najmniej jedną cyfrę");

            RuleFor(u => u).Must(u => u.NewPassword == u.NewPasswordConfirm).WithMessage("Passwords are not equal");
        }
    }
}
