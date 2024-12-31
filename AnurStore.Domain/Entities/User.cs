using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AnurStore.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
