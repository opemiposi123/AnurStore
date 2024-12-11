namespace AnurStore.Application.RequestModel
{
    public class CreateSupplierRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
