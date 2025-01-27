using AnurStore.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class ProductPurchaseItem : BaseEntity 
{
    public string ProductId { get; set; } = default!;

    public Product Product { get; set; } = default!;

    [Column(TypeName = "decimal(18,2)")] 
    public decimal Rate { get; set; } 

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }

    public string ProductPurchaseId { get; set; } = default!;

    public ProductPurchase ProductPurchase { get; set; } = default!;
}