using AnurStore.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class PurchaseItem : BaseEntity
{
    public string ProductId { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public int PacksPurchased { get; set; } 
    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPerPack { get; set; } 
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }
    public string ProductPurchaseId { get; set; } = default!;
    public ProductPurchase ProductPurchase { get; set; } = default!;
}