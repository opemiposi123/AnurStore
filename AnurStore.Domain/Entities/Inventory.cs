using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public string ProductId { get; set; } = default!;
        public Product Product { get; set; } = default!;
        public int QuantityAvailable { get; set; }
        public StockStatus StockStatus { get; set; }
    }
}
