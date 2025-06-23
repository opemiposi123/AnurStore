using AnurStore.Domain.Enums;

namespace AnurStore.Application.DTOs
{
    public class ProductSaleDto
    {
        public string Id { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime SaleDate { get; set; }

        public string? CustomerName { get; set; }

        public decimal? Discount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public string? ReceiptId { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductSaleItemDto> ProductSaleItems { get; set; } = new();
    }
}
