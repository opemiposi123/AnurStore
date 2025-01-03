using AnurStore.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class RecieptItem : BaseEntity
    {
        public int Quantity { get; set; }
        public string? Description { get; set; } = default!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public string RecieptId { get; set; } = default!;
        public Reciept? Reciept { get; set; } = default!;
    }
}
