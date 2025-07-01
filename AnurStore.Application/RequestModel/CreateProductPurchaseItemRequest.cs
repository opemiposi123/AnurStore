namespace AnurStore.Application.RequestModel;

public class CreateProductPurchaseItemRequest
{
    public string ProductId { get; set; } = default!;

    public decimal Rate { get; set; } 

    public int Quantity { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime? ExpirationDate { get; set; } 
}
