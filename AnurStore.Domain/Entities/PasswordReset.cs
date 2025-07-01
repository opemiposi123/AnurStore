using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities
{
    public class PasswordReset : BaseEntity
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public DateTime RequestedAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime UsedAt { get; set; }
    }
}
