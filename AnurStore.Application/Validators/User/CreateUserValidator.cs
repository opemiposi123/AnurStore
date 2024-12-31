using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.Application.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PhoneNumber)
         .Matches(@"^\+?[1-9][0-9]{7,14}$").WithMessage("Invalid phone number format.")
         .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender value.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Invalid role value.");
        }
    }

}
