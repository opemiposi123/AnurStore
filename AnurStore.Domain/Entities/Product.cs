using AnurStore.Domain.Common.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? BarCode { get;set; }
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal PricePerPack { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public string? ProductImageUrl { get; set; }
        public DateTime MFTDate { get; set; } 
        public DateTime ExpiryDate { get; set; }
        public int TotalItemInPack { get; set; }
        public string CategoryId { get; set; } = default!;
        public string BrandId { get; set; } = default!;
        public Category Category { get; set; } = default!;
        public Brand Brand { get; set; } = default!;
        public ProductSize ProductSize { get; set; } = default!;
        public Inventory Inventory { get; set; } = default!;
    }
}
