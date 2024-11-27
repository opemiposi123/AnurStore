using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;

namespace AnurStore.Domain.Entities;

public class Payment : BaseEntity
{
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string TransactionReference { get; set; } = default!;
    public string AccountId { get; set; } = default!;
    public Account? Account { get; set; }
}
