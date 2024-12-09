namespace AnurStore.Application.RequestModel
{
    public class UpdateBrandRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
