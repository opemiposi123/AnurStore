using AnurStore.Domain.Enums;

namespace AnurStore.Application.RequestModel
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!; 
        public string FirstName { get; set; } = default!;
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
    }
}
