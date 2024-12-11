using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = default!;
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
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
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }

}