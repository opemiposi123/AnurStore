using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class Reciept : BaseEntity
    {
        public string RecieptNumber { get; set; } = default!;
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal NetAmount { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerCare { get; set; }

        public string? Notes { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public string ProductSaleId { get; set; } = default!;
        public ProductSale? ProductSale { get; set; }
        public ICollection<RecieptItem> RecieptItems { get; set; } = [];
    }
}