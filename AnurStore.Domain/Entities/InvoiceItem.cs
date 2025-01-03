using AnurStore.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class InvoiceItem : BaseEntity 
{
    public int Quantity { get; set; }
    public string? Description { get; set; } = default!;
    [Column(TypeName = "decimal(18,2)")]   
    public decimal UnitPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    public string InvoiceId { get; set; } = default!;
    public Invoice Invoice { get; set; } = default!;
}