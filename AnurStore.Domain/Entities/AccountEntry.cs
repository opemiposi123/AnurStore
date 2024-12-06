using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class AccountEntry : BaseEntity
    {
        public string AccountId { get; set; } = default!;
        public Account? Account { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }  
        public EntryType EntryType { get; set; }  
        public string TransactionId { get; set; } = default!;
        public Transaction? Transaction { get; set; }  
    }
}
