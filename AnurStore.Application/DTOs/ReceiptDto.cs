using AnurStore.Domain.Enums;

namespace AnurStore.Application.DTOs
{
    public class ReceiptDto
    {
        public string Id { get; set; } = default!;
        public string ReceiptNumber { get; set; } = default!;
        public string? CustomerName { get; set; }
        public string? RecieptNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public List<ReceiptItemDto> ReceiptItems { get; set; } = new();
    }
}
