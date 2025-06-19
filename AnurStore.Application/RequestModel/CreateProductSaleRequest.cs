using AnurStore.Domain.Enums;

namespace AnurStore.Application.RequestModel
{
    public class CreateProductSaleRequest
    {
        public string? CustomerName { get; set; }
        public string? CreatedBy { get; set; }

        public decimal? Discount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<CreateProductSaleItemRequest> ProductSaleItems { get; set; } = new();
    }

}
