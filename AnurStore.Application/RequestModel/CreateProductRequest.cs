using Microsoft.AspNetCore.Http;

namespace AnurStore.Application.RequestModel
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string? BarCode { get; set; } // 5800 * 3.5/100

        public decimal PricePerPack { get; set; }

        public decimal PackPriceMarkup { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitPriceMarkup { get; set; }

        public IFormFile? ProductImage { get; set; } 

        public int TotalItemInPack { get; set; }
        public double ProductSize { get; set; }

        public string CategoryId { get; set; } = default!;
    
        public string? BrandId { get; set; }

        public string UnitId { get; set; } = default!;
    }
}
