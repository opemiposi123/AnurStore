using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.DTOs;
using AnurStore.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _receiptRepository;

    public ReceiptService(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public async Task<(ReceiptDto Receipt, byte[] PdfBytes)> GenerateFromProductSaleAsync(ProductSaleDto sale)
    {
        var receiptNumber = $"INV-{DateTime.UtcNow.Ticks.ToString()[^6..]}";

        var receiptEntity = new Reciept
        {
            RecieptNumber = receiptNumber,
            CustomerName = sale.CustomerName,
            Discount = sale.Discount ?? 0,
            TotalAmount = sale.TotalAmount,
            NetAmount = sale.TotalAmount,
            PaymentMethod = sale.PaymentMethod,
            CreatedOn = DateTime.Now,
            RecieptItems = sale.ProductSaleItems.Select(item => new RecieptItem
            {
                ProductName = item.ProductName ?? "Unknown",
                Quantity = item.Quantity,
                UnitPrice = item.Quantity > 0 ? item.SubTotal / item.Quantity : 0,
                TotalPrice = item.SubTotal,
                CreatedOn = DateTime.Now
            }).ToList()
        };

        await _receiptRepository.GenerateReceiptAsync(receiptEntity);

        byte[] qrImageData = GenerateQrCodeImage(receiptNumber);
        byte[] pdfBytes;

        try
        {
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A6);
                    page.Margin(5);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

                    page.Content().Column(column =>
                    {
                        // Header
                        column.Item().AlignCenter().Text("AnurStore").Bold().FontSize(11);
                        column.Item().AlignCenter().Text("6 Unity Road, Ayoafolabi, Lagos").FontSize(6);
                        column.Item().AlignCenter().Text("Tel: 09068041575").FontSize(6);
                        column.Item().AlignCenter().Text("Email: oseniahoseniahmadkorede@gmail.com").FontSize(6);

                        column.Item().PaddingVertical(1).LineHorizontal(1);
                        column.Item().AlignCenter().Text("SALES INVOICE").Bold().FontSize(8);

                        // Detail section
                        void AddDetail(string label, string value)
                        {
                            column.Item().Row(row =>
                            {
                                row.ConstantItem(60).Text(label).Bold().FontSize(7);
                                row.ConstantItem(5).Text(":");
                                row.RelativeItem().AlignRight().Text(value).FontSize(7);
                            });
                        }

                        AddDetail("INVOICE NO", receiptNumber);
                        AddDetail("CUSTOMER", sale.CustomerName ?? "Guest");
                        AddDetail("DATE", DateTime.Now.ToString("MMM dd, yyyy"));
                        AddDetail("TIME", DateTime.Now.ToString("hh:mm tt"));

                        column.Item().PaddingVertical(1).LineHorizontal(1);

                        // Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(5);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ITEM").Bold().FontSize(7);
                                header.Cell().AlignCenter().Text("QTY").Bold().FontSize(7);
                                header.Cell().AlignRight().Text("₦ AMOUNT").Bold().FontSize(7);
                            });

                            foreach (var item in receiptEntity.RecieptItems)
                            {
                                table.Cell().Element(CellStyle).Text(item.ProductName).FontSize(6.5f);
                                table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString()).FontSize(6.5f);
                                table.Cell().Element(CellStyle).AlignRight().Text($"{item.TotalPrice:N2}").FontSize(6.5f);
                            }

                            IContainer CellStyle(IContainer container) => container.PaddingVertical(1);
                        });

                        column.Item().PaddingVertical(1).LineHorizontal(1);

                        AddDetail("TOTAL", $"₦{receiptEntity.TotalAmount:N2}");
                        AddDetail("DISCOUNT", $"₦{receiptEntity.Discount:N2}");
                        AddDetail("NET AMOUNT", $"₦{receiptEntity.NetAmount:N2}");
                        AddDetail("PAID VIA", sale.PaymentMethod.ToString());

                        column.Item().PaddingVertical(1).LineHorizontal(1);

                        if (qrImageData != null && qrImageData.Length > 0)
                        {
                            column.Item().AlignCenter().Height(35).Image(qrImageData, ImageScaling.FitHeight);
                        }

                        column.Item().AlignCenter().Text("Thank you for your purchase!").Italic().FontSize(6.5f);
                        column.Item().AlignCenter().Text("© 2025 AnurStore").FontSize(6);
                    });
                });
            });

            pdfBytes = pdf.GeneratePdf();
        }
        catch (Exception ex)
        {
            throw new Exception("PDF layout failed. Ensure all values and images are valid.", ex);
        }

        var receiptDto = new ReceiptDto
        {
            Id = receiptEntity.Id,
            RecieptNumber = receiptEntity.RecieptNumber,
            CustomerName = receiptEntity.CustomerName,
            TotalAmount = receiptEntity.TotalAmount,
            NetAmount = receiptEntity.NetAmount,
            Discount = receiptEntity.Discount,
            PaymentMethod = receiptEntity.PaymentMethod,
            ReceiptItems = receiptEntity.RecieptItems.Select(r => new ReceiptItemDto
            {
                ProductName = r.ProductName,
                Quantity = r.Quantity,
                UnitPrice = r.UnitPrice,
                TotalPrice = r.TotalPrice
            }).ToList()
        };

        return (receiptDto, pdfBytes);
    }

    private byte[] GenerateQrCodeImage(string text)
    {
        var qrWriter = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions { Height = 150, Width = 150, Margin = 1 }
        };

        var pixelData = qrWriter.Write(text);

        using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, pixelData.Width, pixelData.Height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppRgb
        );

        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
        bitmap.UnlockBits(bitmapData);

        using var stream = new MemoryStream();
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        return stream.ToArray();
    }
}

