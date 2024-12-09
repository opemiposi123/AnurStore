using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Domain.Entities
{
    public class Account : BaseEntity 
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } 
        public AccountType AccountType { get; set; }  
    }
}
