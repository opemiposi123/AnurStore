using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace AnurStore.Domain.Entities;

public class ProductSale : BaseEntity
{
    [Column(TypeName = "money")]
    public decimal TotalAmount { get; set; }

    public DateTime SaleDate { get; set; } = DateTime.Now;

    public string? CustomerName { get; set; } 

    [Column(TypeName = "money")]
    public decimal? Discount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public ICollection<ProductSaleItem> ProductSaleItems { get; set; } = [];
    public string ReceiptId { get; set; } = default!;
    public Reciept Receipt { get; set; } = default!;
}

