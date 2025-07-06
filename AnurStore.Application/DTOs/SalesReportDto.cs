namespace AnurStore.Application.DTOs
{
    public class SalesReportDto
    {
        public decimal TotalCash { get; set; }
        public decimal TotalPOS { get; set; }
        public decimal TotalTransfer { get; set; }
        public decimal TotalCheque { get; set; }

        public List<SaleRecordDto> Sales { get; set; } = new();
    }

    public class SaleRecordDto
    {
        public string OrderId { get; set; } = default!;
        public string CustomerName { get; set; } = "Guest";
        public string SoldBy { get; set; } = default!;
        public decimal Amount { get; set; }
        public string PaymentType { get; set; } = default!;
        public string Status { get; set; } = "Paid";
    }

}
