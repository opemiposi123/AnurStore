using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Application.DTOs
{
    public class ProductPurchaseDto
    {
        [Column(TypeName = "money")]
        public decimal Total { get; set; }

        [Column(TypeName = "money")]
        public decimal? Discount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string SupplierId { get; set; } = default!;

        public string SupplierName { get; set; } = default!; 

        public ICollection<ProductPurchaseItemDto> PurchaseItems { get; set; } = [];

        public DateTime PurchaseDate { get; set; }

        public bool IsAddedToInventory { get; set; }
    }
}
