using AnurStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Domain.Entities
{
    public class Account
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } 
        public AccountType AccountType { get; set; }  
    }
}
