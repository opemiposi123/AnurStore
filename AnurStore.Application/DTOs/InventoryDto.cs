using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using MassTransit;

namespace AnurStore.Application.DTOs
{
    public class InventoryDto
    {
        public string Id { get; set; } = NewId.Next().ToSequentialGuid().ToString();
        public string ProductId { get; set; } = default!;
        public string? ProductName { get; set; } 
        public string? ProductBrand { get; set; } 
        public string? ProductCategory { get; set; }  
        public decimal? PackPrice { get; set; }   
        public decimal? UnitPrice { get; set; }  
        public string? ProductSize { get; set; } 
        public int QuantityAvailable { get; set; }
        public StockStatus StockStatus { get; set; } 
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
