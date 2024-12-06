using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities;

public class Payment : BaseEntity
{
    public DateTime PaymentDate { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string TransactionReference { get; set; } = default!;
    public string AccountId { get; set; } = default!;
    public Account? Account { get; set; }
}
