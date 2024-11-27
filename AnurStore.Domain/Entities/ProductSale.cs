using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
namespace AnurStore.Domain.Entities;

public class ProductSale : BaseEntity
{
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public string? CustomerName { get; set; }
    public decimal Discount { get; set; }
    public string ReceiptNumber { get; set; } = default!;
    public PaymentMethod PaymentMethod { get; set; }
    public string UserId { get; set; } = default!;
    public User? User { get; set; }
    public ICollection<ProductSaleItem> ProductSaleItems { get; set; } = [];
}
