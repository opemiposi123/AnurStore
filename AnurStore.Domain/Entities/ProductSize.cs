using AnurStore.Domain.Common.Contracts;

namespace AnurStore.Domain.Entities;

public class ProductSize : BaseEntity
{
    public double Size { get; set; }

    public string ProductId { get; set; } = default!;

    public Product Product { get; set; } = default!;

    public string ProductUnitId { get; set; } = default!;

    public ProductUnit ProductUnit { get; set; } = default!;
}
