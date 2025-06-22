using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class ProductPurchase : BaseEntity
{
    public string Batch { get; set; } 

    [Column(TypeName = "money")]
    public decimal Total { get; set; } 

    [Column(TypeName = "money")]
    public decimal? Discount { get; set; }

    public string SupplierId { get; set; } = default!;

    public Supplier Supplier { get; set; } = default!;

    public ICollection<ProductPurchaseItem> PurchaseItems { get; set; } = [];

    public DateTime PurchaseDate { get; set; }

    public bool IsAddedToInventory { get; set; }    
}

