namespace AnurStore.Application.RequestModel
{
    public class CreateBrandRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}