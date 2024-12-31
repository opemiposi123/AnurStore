using AnurStore.Application.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.Validators.ProductUnit
{
    public class CreateProductUnitValidator : AbstractValidator<CreateProductUnitRequest>
    {
        public CreateProductUnitValidator() 
        {
            RuleFor(x => x.Name) 
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
        }
    }
}
