using AnurStore.Domain.Enums;

namespace AnurStore.Application.DTOs
{
    public class ProductSaleItemDto
    {
        public Guid Id { get; set; }

        public string ProductId { get; set; } 

        public string? ProductName { get; set; } 

        public int Quantity { get; set; }

        public ProductUnitType ProductUnitType { get; set; }

        public decimal SubTotal { get; set; }

        public string ProductSaleId { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
