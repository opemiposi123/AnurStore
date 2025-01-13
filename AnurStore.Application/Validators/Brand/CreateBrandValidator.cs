using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.Application.Validators.Brand
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
    {
        public CreateBrandValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(25).WithMessage("Name must not exceed 25 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
        }
    }
}
