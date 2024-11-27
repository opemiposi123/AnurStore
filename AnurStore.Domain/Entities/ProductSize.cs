using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class ProductSize : BaseEntity
    {
        public decimal Quantity { get; set; }
        public string ProductUnitId { get; set; } = default!; 
        public ProductUnit ProductUnit { get; set; } = default!;
        public string ProductId { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
