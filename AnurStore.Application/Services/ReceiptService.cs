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

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _receiptRepository;

    public ReceiptService(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public async Task<(ReceiptDto Receipt, byte[] PdfBytes)> GenerateFromProductSaleAsync(ProductSaleDto sale)
    {
        if (sale == null || sale.ProductSaleItems == null || !sale.ProductSaleItems.Any())
            throw new ArgumentException("Sale or Sale Items are empty. Cannot generate receipt.");

        var receiptNumber = $"ASR-{DateTime.UtcNow:yyyyMMdd}-{DateTime.UtcNow.Ticks.ToString()[^6..]}";

        var receiptEntity = new Reciept
        {
            RecieptNumber = receiptNumber,
            CustomerName = sale.CustomerName,
            Discount = sale.Discount ?? 0,
            TotalAmount = sale.TotalAmount,
            NetAmount = sale.TotalAmount - (sale.Discount ?? 0),
            PaymentMethod = sale.PaymentMethod,
            CreatedOn = DateTime.Now,
            RecieptItems = sale.ProductSaleItems.Select(item => new RecieptItem
            {
                ProductName = item.ProductName ?? "Unknown Product",
                Quantity = item.Quantity,
                UnitPrice = item.Quantity > 0 ? item.SubTotal / item.Quantity : 0,
                TotalPrice = item.SubTotal,
                CreatedOn = DateTime.Now
            }).ToList()
        };

        await _receiptRepository.GenerateReceiptAsync(receiptEntity);

        byte[] qrImageData = GenerateQrCodeImage(receiptNumber);
        byte[] pdfBytes = GeneratePdfReceipt(receiptEntity, sale, qrImageData);

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

    private byte[] GeneratePdfReceipt(Reciept receipt, ProductSaleDto sale, byte[] qrImageData)
    {
        try
        {
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    const float mmToPt = 2.83465f; 
                    float width = 120f * mmToPt;
                    float height = 200f * mmToPt;

                    page.Size(width, height);
                    page.PageColor(Colors.White);
                    page.Margin(8f * mmToPt);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Content().Column(column =>
                    {
                        // Header Section - Compact but wider
                        column.Item().Background(Colors.Grey.Lighten4).Padding(6).Column(headerColumn =>
                        {
                            headerColumn.Item().AlignCenter().Text("ANUR STORE").Bold().FontSize(16).FontColor(Colors.Blue.Darken2);
                            headerColumn.Item().AlignCenter().Text("Premium Quality Products").FontSize(9).Italic().FontColor(Colors.Grey.Darken1);
                            headerColumn.Item().PaddingTop(3).AlignCenter().Text("📍 44 Idowu Buhari Str, Robiyan, Ogun").FontSize(8);
                            headerColumn.Item().AlignCenter().Text("📞 08051550404").FontSize(8);
                            headerColumn.Item().AlignCenter().Text("📧 anurstore@gmail.com").FontSize(8);
                        });

                        column.Item().PaddingVertical(4);

                        // Receipt Header
                        column.Item().Background(Colors.Blue.Lighten4).Padding(4).AlignCenter().Text("SALES RECEIPT").Bold().FontSize(13).FontColor(Colors.Blue.Darken2);

                        column.Item().PaddingVertical(4);

                        // Customer and Transaction Details in a more compact row format
                        column.Item().Column(detailsColumn =>
                        {
                            // First row: Customer and Payment Method
                            detailsColumn.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Customer: {receipt.CustomerName ?? "Walk-in Customer"}").FontSize(9).Bold();
                                row.RelativeItem().AlignRight().Text($"Payment: {sale.PaymentMethod}").FontSize(9);
                            });

                            // Second row: Date and Receipt Number
                            detailsColumn.Item().PaddingTop(2).Row(row =>
                            {
                                row.RelativeItem().Text($"Date: {DateTime.Now:MMM dd, yyyy HH:mm}").FontSize(9);
                                row.RelativeItem().AlignRight().Text($"Receipt #: {receipt.RecieptNumber}").FontSize(9).Bold();
                            });
                        });

                        column.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        // Items Section - Condensed format for many items
                        column.Item().Column(itemsColumn =>
                        {
                            // Header
                            itemsColumn.Item().Background(Colors.Blue.Lighten3).Padding(3).Row(headerRow =>
                            {
                                headerRow.ConstantItem(25).Text("#").Bold().FontSize(9);
                                headerRow.RelativeItem(4).Text("ITEM").Bold().FontSize(9);
                                headerRow.ConstantItem(35).AlignCenter().Text("QTY").Bold().FontSize(9);
                                headerRow.ConstantItem(45).AlignRight().Text("UNIT").Bold().FontSize(9);
                                headerRow.ConstantItem(55).AlignRight().Text("TOTAL").Bold().FontSize(9);
                            });

                            // Items - Compact single-line format for each item
                            int serialNumber = 1;
                            foreach (var item in receipt.RecieptItems)
                            {
                                // Alternate row colors for better readability
                                var backgroundColor = serialNumber % 2 == 0 ? Colors.Grey.Lighten5 : Colors.White;

                                itemsColumn.Item().Background(backgroundColor).Padding(2).Row(itemRow =>
                                {
                                    itemRow.ConstantItem(25).Text($"{serialNumber}").FontSize(9);
                                    itemRow.RelativeItem(4).Text(TruncateText(item.ProductName, 25)).FontSize(9);
                                    itemRow.ConstantItem(35).AlignCenter().Text($"{item.Quantity:N0}").FontSize(9);
                                    itemRow.ConstantItem(45).AlignRight().Text($"₦{item.UnitPrice:N2}").FontSize(9);
                                    itemRow.ConstantItem(55).AlignRight().Text($"₦{item.TotalPrice:N2}").FontSize(9).Bold();
                                });

                                serialNumber++;
                            }
                        });

                        column.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        // Totals Section - More compact
                        column.Item().Column(totalsColumn =>
                        {
                            var subtotal = receipt.TotalAmount + receipt.Discount;

                            // Show subtotal if there's a discount
                            if (receipt.Discount > 0)
                            {
                                totalsColumn.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("Subtotal:").FontSize(10);
                                    row.ConstantItem(80).AlignRight().Text($"₦{subtotal:N2}").FontSize(10);
                                });

                                totalsColumn.Item().Row(row =>
                                {
                                    row.RelativeItem().Text("Discount:").FontSize(10);
                                    row.ConstantItem(80).AlignRight().Text($"-₦{receipt.Discount:N2}").FontSize(10).FontColor(Colors.Red.Medium);
                                });
                            }

                            // Final Total with emphasis
                            totalsColumn.Item().PaddingTop(3).Background(Colors.Blue.Lighten4).Padding(4).Row(totalRow =>
                            {
                                totalRow.RelativeItem().Text("TOTAL AMOUNT:").Bold().FontSize(12);
                                totalRow.ConstantItem(90).AlignRight().Text($"₦{receipt.NetAmount:N2}").Bold().FontSize(14).FontColor(Colors.Green.Darken1);
                            });
                        });

                        column.Item().PaddingVertical(6);

                        // QR Code Section - Smaller to save space
                        if (qrImageData != null && qrImageData.Length > 0)
                        {
                            column.Item().AlignCenter().Column(qrColumn =>
                            {
                                qrColumn.Item().Text("Scan for Verification").FontSize(8).FontColor(Colors.Grey.Darken1).AlignCenter();
                                qrColumn.Item().PaddingTop(3).Height(35).AlignCenter().Image(qrImageData, ImageScaling.FitHeight);
                            });
                        }

                        column.Item().PaddingVertical(4);

                        // Footer - Condensed
                        column.Item().Background(Colors.Grey.Lighten4).Padding(4).Column(footerColumn =>
                        {
                            footerColumn.Item().AlignCenter().Text("Thank you for choosing AnurStore!").Bold().FontSize(10).FontColor(Colors.Blue.Darken2);
                            footerColumn.Item().AlignCenter().Text("Your satisfaction is our priority").FontSize(8).Italic().FontColor(Colors.Grey.Darken1);
                            footerColumn.Item().PaddingTop(2).AlignCenter().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}").FontSize(7).FontColor(Colors.Grey.Medium);
                        });
                    });
                });
            });

            return pdf.GeneratePdf();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate PDF receipt. Please ensure all data is valid.", ex);
        }
    }

    // Helper method to truncate long product names
    private string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength - 3) + "...";
    }

    private byte[] GenerateQrCodeImage(string text)
    {
        try
        {
            var encodingOptions = new EncodingOptions
            {
                Height = 120, // Smaller QR code
                Width = 120,
                Margin = 1
            };

            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M);
            encodingOptions.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = encodingOptions
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
        catch (Exception ex)
        {
            Console.WriteLine($"QR Code generation failed: {ex.Message}");
            return new byte[0];
        }
    }
}