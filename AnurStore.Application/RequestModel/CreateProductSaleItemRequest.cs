using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AnurStore.Application.RequestModel
{
    public class CreateProductSaleItemRequest
    {
        public string ProductId { get; set; } = default!;
        public string CreatedBy { get; set; } 

        public int Quantity { get; set; }

        public ProductUnitType ProductUnitType { get; set; }

        public decimal SubTotal { get; set; }
    }
}
