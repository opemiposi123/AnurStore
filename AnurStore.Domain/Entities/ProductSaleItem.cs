using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities;

public class ProductSaleItem
{
    public string ProductId { get; set; } = default!;
    public Product? Product { get; set; }
    public int Quantity { get; set; } 
    public ProductUnitType ProductUnitType { get; set; }
    public decimal SubTotal { get; set; } 
    public string? ProductSaleId { get; set; }
    public ProductSale? ProductSale { get; set; }
}