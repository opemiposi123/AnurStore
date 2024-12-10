using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.Application.Validators.Brand
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
    {
        public UpdateBrandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
        }
    }
}
