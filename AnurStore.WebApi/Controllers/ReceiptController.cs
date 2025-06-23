using AnurStore.Application.DTOs;
using AnurStore.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using AnurStore.Application.Abstractions.Repositories;

namespace AnurStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        /// <summary>
        /// Generate a receipt from a given product sale.
        /// </summary>
        /// <param name="saleDto">The DTO containing sale information.</param>
        /// <returns>PDF receipt file.</returns>
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateReceipt([FromBody] ProductSaleDto saleDto)
        {
            if (saleDto == null || saleDto.ProductSaleItems == null || !saleDto.ProductSaleItems.Any())
                return BadRequest("Invalid sale data. Ensure sale contains at least one item.");

            try
            {
                var (receipt, pdfBytes) = await _receiptService.GenerateFromProductSaleAsync(saleDto);

                // Return the PDF as a downloadable file
                return File(pdfBytes, "application/pdf", $"{receipt.RecieptNumber}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate receipt: {ex.Message}");
            }
        }
    }
}
