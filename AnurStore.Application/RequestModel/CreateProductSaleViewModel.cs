using AnurStore.Application.DTOs;

namespace AnurStore.Application.RequestModel
{
    public class CreateProductSaleViewModel
    {
        public CreateProductSaleRequest SaleRequest { get; set; } = new();
        public IEnumerable<ProductDto> AvailableProducts { get; set; } = new List<ProductDto>();
    }
}