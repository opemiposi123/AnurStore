using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities;

public class ProductPurchase : BaseEntity
{
    public int Quantity { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    public decimal Discount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string SupplierId { get; set; } = default!;
    public string ProductId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public Supplier Supplier { get; set; } = default!;
    public User User { get; set; } = default!;
    public ICollection<Product> Products { get; set; } = [];
}
