using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class ProductPurchase : BaseEntity
{
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; } 
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string SupplierId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public Supplier Supplier { get; set; } = default!;
    public User User { get; set; } = default!;
    public ICollection<ProductPurchaseItem> PurchaseItems { get; set; } = [];
}

