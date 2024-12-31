namespace AnurStore.Application.RequestModel
{
    public class CreateProductUnitRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}