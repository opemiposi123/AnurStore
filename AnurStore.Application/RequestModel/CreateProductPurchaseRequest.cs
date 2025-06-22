using System.ComponentModel.DataAnnotations.Schema;

namespace AnurStore.Application.RequestModel
{
    public class CreateProductPurchaseRequest
    {
        public string Batch { get; set; } = default!;

        [Column(TypeName = "money")] 
        public decimal Total { get; set; } 

        [Column(TypeName = "money")]
        public decimal? Discount { get; set; }

        public string SupplierId { get; set; } = default!;

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public bool IsAddedToInventory { get; set; } = true;

        public List<CreateProductPurchaseItemRequest> PurchaseItems { get; set; } = new();
    }

}
