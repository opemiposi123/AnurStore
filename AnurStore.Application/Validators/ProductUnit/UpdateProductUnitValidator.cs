﻿using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.Application.Validators.ProductUnit
{
    public class UpdateProductUnitValidator : AbstractValidator<UpdateProductUnitRequest>
    {
        public UpdateProductUnitValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
        }
    }
}