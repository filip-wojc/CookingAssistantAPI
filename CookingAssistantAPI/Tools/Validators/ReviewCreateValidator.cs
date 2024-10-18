using CookingAssistantAPI.DTO.Reviews;
using FluentValidation;

namespace CookingAssistantAPI.Tools.Validators
{
    public class ReviewCreateValidator : AbstractValidator<ReviewCreateDTO>
    {
        public ReviewCreateValidator()
        {
            RuleFor(r => r.Value).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
        }
    }
}
