using AnurStore.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class ProductSize : BaseEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public string ProductUnitId { get; set; } = default!; 
        public ProductUnit ProductUnit { get; set; } = default!;
        public string ProductId { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
