using AnurStore.Application.DTOs;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IReceiptService
    {
        Task<(ReceiptDto Receipt, byte[] PdfBytes)> GenerateFromProductSaleAsync(ProductSaleDto sale);
    }
}
