namespace AnurStore.Application.DTOs
{
    public class ReceiptItemDto
    {
        public string Id { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
