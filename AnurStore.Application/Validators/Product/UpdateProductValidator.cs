﻿using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.Application.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.BarCode)
                .MaximumLength(50).WithMessage("Barcode must not exceed 50 characters.");

            RuleFor(x => x.PricePerPack)
                .GreaterThanOrEqualTo(0).WithMessage("Price per pack cannot be negative.");

            RuleFor(x => x.PackPriceMarkup)
                .GreaterThanOrEqualTo(0).WithMessage("Pack price markup cannot be negative.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");

            RuleFor(x => x.TotalItemInPack)
                .GreaterThanOrEqualTo(0).WithMessage("Total items in pack cannot be negative.");

            RuleFor(x => x.ProductSize)
                .GreaterThan(0).WithMessage("Product size must be greater than 0.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.UnitId)
                .NotEmpty().WithMessage("Unit ID is required.")
                .When(x => x.UnitId != null);

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Brand ID is required.")
                .When(x => !string.IsNullOrWhiteSpace(x.BrandId));

            RuleFor(x => x.ProductImage)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Product image must be valid if provided.");
        }
    }
}
