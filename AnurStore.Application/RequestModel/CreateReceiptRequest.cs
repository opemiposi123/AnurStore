using AnurStore.Domain.Enums;

namespace AnurStore.Application.RequestModel
{
    public class CreateReceiptRequest
    {
        public string? CustomerName { get; set; }
        public string? CreatedBy { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CreateReceiptItemRequest> ReceiptItems { get; set; } = new();
    }
}
