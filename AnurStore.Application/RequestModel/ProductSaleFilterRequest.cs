using AnurStore.Domain.Enums;

namespace AnurStore.Application.RequestModel
{
    public class ProductSaleFilterRequest : PagedRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CustomerName { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }

}
