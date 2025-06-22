using AnurStore.Application.DTOs;
using AnurStore.Application.Wrapper;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IReceiptService
    {
        Task<(ReceiptDto Receipt, byte[] PdfBytes)> GenerateFromProductSaleAsync(ProductSaleDto sale);
    }
}
