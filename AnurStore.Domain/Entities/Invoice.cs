using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = default!;
        public DateTime InvoiceDate { get; set; } = DateTime.Now; 
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; } 
        public decimal NetAmount { get; set; } 
        public string? CustomerName { get; set; } 
        public PaymentMethod PaymentMethod { get; set; } 
        public string? Notes { get; set; } 
    }

}
