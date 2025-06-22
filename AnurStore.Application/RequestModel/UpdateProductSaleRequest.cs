using AnurStore.Domain.Enums;
namespace AnurStore.Application.RequestModel
{
    public class UpdateProductSaleRequest
    {
        public string Id { get; set; } = default!;
        public string? CustomerName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal? Discount { get; set; }
        public List<CreateProductSaleItemRequest> ProductSaleItems { get; set; } = new();
    }

}
