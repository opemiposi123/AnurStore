using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace AnurStore.Domain.Entities;

public class ProductSale : BaseEntity
{
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public string? CustomerName { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }
    public string ReceiptNumber { get; set; } = default!;
    public PaymentMethod PaymentMethod { get; set; }
    public string InvoiceId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public User? User { get; set; }
    public Invoice Invoice { get; set; } = default!;
    public ICollection<ProductSaleItem> ProductSaleItems { get; set; } = [];
    public string InventoryId { get; set; } = default!;
    public Inventory? Inventory { get; set; }
}

