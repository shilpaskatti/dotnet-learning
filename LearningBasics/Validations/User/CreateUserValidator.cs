using FluentValidation;
using LearningBasics.DTOs.Request;

namespace LearningBasics.Validations.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(m=>m.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(3,10).WithMessage("Minimum length is 3 and max is 10.");
            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last name is required");
            RuleFor(m => m.Subjects)
                .NotEmpty().WithMessage("Subjects is required");
        }
    }
}
