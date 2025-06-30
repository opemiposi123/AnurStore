using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string? BarCode { get;set; } 

        public decimal? PricePerPack { get; set; } 

        public decimal PackPriceMarkup { get; set; }

        public decimal? UnitPrice { get; set; } 

        public string? ProductImageUrl { get; set; } 

        public int TotalItemInPack { get; set; }

        public string CategoryId { get; set; } = default!;

        public string? BrandId { get; set; }

        public Category Category { get; set; } = default!;

        public Brand? Brand { get; set; } 
        
        public ProductSize ProductSize { get; set; } = default!;

        public Inventory Inventory { get; set; } = default!;
    }
}
