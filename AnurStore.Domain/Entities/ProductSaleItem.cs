using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class ProductSaleItem
{
    public string ProductId { get; set; } = default!;
    public Product? Product { get; set; }
    public int Quantity { get; set; } 
    public ProductUnitType ProductUnitType { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; } 
    public string ProductSaleId { get; set; } = default!;
    public ProductSale? ProductSale { get; set; }
}