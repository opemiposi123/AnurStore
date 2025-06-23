using AnurStore.Application.RequestModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.Validators.ProductPurchase
{
    public class ProductPurchaseItemValidator : AbstractValidator<CreateProductPurchaseItemRequest>
    {
        public ProductPurchaseItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product is required.");

            RuleFor(x => x.Rate)
                .GreaterThan(0).WithMessage("Rate must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.TotalCost)
                .GreaterThan(0).WithMessage("Total cost must be greater than 0.");
        }
    }

}
