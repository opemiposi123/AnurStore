namespace AnurStore.Application.RequestModel
{
    public class CreateReceiptItemRequest
    {
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
