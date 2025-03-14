using AnurStore.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Application.DTOs
{
    public class ProductPurchaseItemDto
    {
        public string ProductId { get; set; } = default!;

        public string ProductName { get; set; } = default!;
        public string ProductCategory { get; set; } = default!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; } 
        public int Quantity { get; set; } 

        [Column(TypeName = "decimal(18,2)")] 
        public decimal TotalCost { get; set; }

        public string ProductPurchaseId { get; set; } = default!;
    }
}
