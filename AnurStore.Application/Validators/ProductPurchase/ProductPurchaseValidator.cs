using AnurStore.Application.RequestModel;
using FluentValidation;

namespace AnurStore.Application.Validators.ProductPurchase
{
    public class ProductPurchaseValidator : AbstractValidator<CreateProductPurchaseRequest>
    {
        public ProductPurchaseValidator()
        {
            RuleFor(x => x.Batch)
                .NotEmpty().WithMessage("Batch is required.")
                .MaximumLength(100);

            RuleFor(x => x.SupplierId)
                .NotEmpty().WithMessage("Supplier is required.");

            RuleFor(x => x.Total)
                .GreaterThan(0).WithMessage("Total must be greater than 0.");

            RuleFor(x => x.PurchaseDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Purchase date cannot be in the future.");

            RuleFor(x => x.PurchaseItems)
                .NotEmpty().WithMessage("At least one purchase item is required.");

            RuleForEach(x => x.PurchaseItems)
                .SetValidator(new ProductPurchaseItemValidator());
        }
    }

}
