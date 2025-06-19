using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;

namespace AnurStore.Application.Services
{
    public class ProductSaleService : IProductSaleService
    {
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReceiptService _receiptService;

        public ProductSaleService(IProductSaleRepository productSaleRepository, IProductRepository productRepository, IReceiptService receiptService)
        {
            _productSaleRepository = productSaleRepository;
            _productRepository = productRepository;
            _receiptService = receiptService;
        }

        public async Task<BaseResponse<byte[]>> AddProductSale(CreateProductSaleRequest request)
        {
            try
            {
                decimal totalAmount = 0;
                var saleItems = new List<ProductSaleItem>();

                foreach (var item in request.ProductSaleItems)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);

                    if (product == null)
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Product with ID {item.ProductId} not found"
                        };

                    if (item.ProductUnitType == ProductUnitType.SingleUnit && product.UnitPrice == null)
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Product {product.Name} does not support unit pricing"
                        };

                    if (item.ProductUnitType == ProductUnitType.Pack && product.PricePerPack == null)
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Product {product.Name} does not support pack pricing"
                        };

                    decimal subTotal;

                    if (item.ProductUnitType == ProductUnitType.SingleUnit)
                    {
                        subTotal = product.UnitPrice!.Value * item.Quantity;
                    }
                    else
                    {
                        if (item.Quantity % product.TotalItemInPack == 0)
                        {
                            int numberOfPacks = item.Quantity / product.TotalItemInPack;
                            subTotal = product.PricePerPack!.Value * numberOfPacks;
                        }
                        else
                        {
                            decimal pricePerUnitFromPack = product.PricePerPack!.Value / product.TotalItemInPack;
                            subTotal = pricePerUnitFromPack * item.Quantity;
                        }
                    }

                    if (product.Inventory.QuantityAvailable < item.Quantity)
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Insufficient stock for {product.Name}. Only {product.Inventory.QuantityAvailable} available."
                        };

                    product.Inventory.QuantityAvailable -= item.Quantity;

                    saleItems.Add(new ProductSaleItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        ProductUnitType = item.ProductUnitType,
                        SubTotal = subTotal,
                        CreatedBy = item.CreatedBy
                    });

                    totalAmount += subTotal;

                    await _productRepository.UpdateProduct(product);
                }

                decimal discount = request.Discount ?? 0;

                totalAmount -= discount;

                var productSale = new ProductSale
                {
                    CustomerName = request.CustomerName,
                    PaymentMethod = request.PaymentMethod,
                    Discount = discount,
                    TotalAmount = totalAmount,
                    ProductSaleItems = saleItems,
                    SaleDate = DateTime.Now,
                    CreatedBy = request.CreatedBy

                };

                await _productSaleRepository.AddProductSaleAsync(productSale);

                var saleDto = new ProductSaleDto
                {
                    CustomerName = productSale.CustomerName,
                    PaymentMethod = productSale.PaymentMethod,
                    Discount = productSale.Discount,
                    TotalAmount = productSale.TotalAmount,
                    SaleDate = productSale.SaleDate,
                    ProductSaleItems = saleItems.Select(si => new ProductSaleItemDto
                    {
                        ProductName = si.Product?.Name,
                        Quantity = si.Quantity,
                        SubTotal = si.SubTotal,
                    }).ToList()
                };

                var (receipt, pdfBytes) = await _receiptService.GenerateFromProductSaleAsync(saleDto);

                productSale.ReceiptId = receipt.Id;
                await _productSaleRepository.UpdateAsync(productSale);
                return new BaseResponse<byte[]>
                {
                    Status = true,
                    Message = "Product sale recorded successfully",
                    Data = pdfBytes
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<byte[]>
                {
                    Status = false,
                    Message = $"Failed to record product sale: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<ProductSaleDto>> GetProductSaleById(string productSaleId)
        {
            var productSale = await _productSaleRepository.GetProductSaleByIdAsync(productSaleId);

            if (productSale == null)
            {
                return new BaseResponse<ProductSaleDto>
                {
                    Status = false,
                    Message = $"Product sale with ID {productSaleId} was not found."
                };
            }

            var productSaleDto = new ProductSaleDto
            {
                Id = productSale.Id,
                CustomerName = productSale.CustomerName,
                PaymentMethod = productSale.PaymentMethod,
                SaleDate = productSale.SaleDate,
                TotalAmount = productSale.TotalAmount,
                Discount = productSale.Discount,
                CreatedBy = productSale.CreatedBy,
                CreatedOn = productSale.CreatedOn,
                LastModifiedBy = productSale.LastModifiedBy,
                LastModifiedOn = productSale.LastModifiedOn,
                ProductSaleItems = productSale.ProductSaleItems.Select(item => new ProductSaleItemDto
                {
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    ProductUnitType = item.ProductUnitType,
                    SubTotal = item.SubTotal,
                    ProductId = item.ProductId,
                    ProductSaleId = item.ProductSaleId,
                }).ToList()
            };

            return new BaseResponse<ProductSaleDto>
            {
                Status = true,
                Message = "Product sale retrieved successfully",
                Data = productSaleDto
            };
        }


        public async Task<BaseResponse<PagedResponse<ProductSaleDto>>> GetAllProductSalesPagedAsync(PaginationRequest request)
        {
            var productSales = await _productSaleRepository.GetAllProductSalesAsync();

            if (productSales == null || !productSales.Any())
            {
                return new BaseResponse<PagedResponse<ProductSaleDto>>
                {
                    Status = false,
                    Message = "No product sales found."
                };
            }

            int totalRecords = productSales.Count;

            var pagedSales = productSales
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(sale => new ProductSaleDto
                {
                    CustomerName = sale.CustomerName,
                    PaymentMethod = sale.PaymentMethod,
                    SaleDate = sale.SaleDate,
                    TotalAmount = sale.TotalAmount,
                    Discount = sale.Discount,
                    CreatedBy = sale.CreatedBy,
                    CreatedOn = sale.CreatedOn,
                    LastModifiedBy = sale.LastModifiedBy,
                    LastModifiedOn = sale.LastModifiedOn,
                    ProductSaleItems = sale.ProductSaleItems.Select(item => new ProductSaleItemDto
                    {
                        ProductName = item.Product?.Name,
                        Quantity = item.Quantity,
                        ProductUnitType = item.ProductUnitType,
                        SubTotal = item.SubTotal,
                        ProductId = item.ProductId,
                        ProductSaleId = item.ProductSaleId,
                    }).ToList()
                }).ToList();

            return new BaseResponse<PagedResponse<ProductSaleDto>>
            {
                Status = true,
                Message = "Product sales retrieved successfully.",
                Data = new PagedResponse<ProductSaleDto>
                {
                    Data = pagedSales,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalRecords = totalRecords
                }
            };
        }



        public async Task<BaseResponse<PagedResponse<ProductSaleDto>>> GetFilteredProductSalesPagedAsync(ProductSaleFilterRequest filter)
        {
            var query = await _productSaleRepository.GetAllProductSalesAsync();

            if (filter.StartDate.HasValue)
                query = query.Where(s => s.SaleDate >= filter.StartDate.Value).ToList();

            if (filter.EndDate.HasValue)
                query = query.Where(s => s.SaleDate <= filter.EndDate.Value).ToList();

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
                query = query.Where(s => s.CustomerName != null && s.CustomerName.Contains(filter.CustomerName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (filter.PaymentMethod.HasValue)
                query = query.Where(s => s.PaymentMethod == filter.PaymentMethod).ToList();

            var totalRecords = query.Count;

            var pagedSales = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(sale => new ProductSaleDto
                {
                    CustomerName = sale.CustomerName,
                    PaymentMethod = sale.PaymentMethod,
                    SaleDate = sale.SaleDate,
                    TotalAmount = sale.TotalAmount,
                    Discount = sale.Discount,
                    CreatedBy = sale.CreatedBy,
                    CreatedOn = sale.CreatedOn,
                    LastModifiedBy = sale.LastModifiedBy,
                    LastModifiedOn = sale.LastModifiedOn,
                    ProductSaleItems = sale.ProductSaleItems.Select(item => new ProductSaleItemDto
                    {
                        ProductName = item.Product?.Name,
                        Quantity = item.Quantity,
                        ProductUnitType = item.ProductUnitType,
                        SubTotal = item.SubTotal,
                        ProductId = item.ProductId,
                        ProductSaleId = item.ProductSaleId
                    }).ToList()
                }).ToList();

            return new BaseResponse<PagedResponse<ProductSaleDto>>
            {
                Status = true,
                Message = "Sales retrieved successfully.",
                Data = new PagedResponse<ProductSaleDto>
                {
                    Data = pagedSales,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalRecords = totalRecords
                }
            };
        }



        public async Task<BaseResponse<bool>> CancelProductSaleAsync(string saleId)
        {
            var sale = await _productSaleRepository.GetProductSaleByIdAsync(saleId);

            if (sale == null)
                return new BaseResponse<bool> { Status = false, Message = "Sale not found." };

            if (sale.IsDeleted)
                return new BaseResponse<bool> { Status = false, Message = "Sale is already canceled." };

            foreach (var item in sale.ProductSaleItems)
            {
                item.Product.Inventory.QuantityAvailable += item.Quantity;
            }

            sale.IsDeleted = true;
            sale.DeletedOn = DateTime.Now;

            var result = _productSaleRepository.UpdateAsync(sale);

            return new BaseResponse<bool> { Status = true, Message = "Sale canceled successfully.", Data = true };
        }


        public async Task<BaseResponse<bool>> UpdateProductSaleAsync(UpdateProductSaleRequest request)
        {
            var sale = await _productSaleRepository.GetProductSaleByIdAsync(request.SaleId);

            if (sale == null)
                return new BaseResponse<bool> { Status = false, Message = "Sale not found." };

            if (sale.IsDeleted)
                return new BaseResponse<bool> { Status = false, Message = "Cannot update a canceled sale." };

            // Revert old inventory
            foreach (var item in sale.ProductSaleItems)
            {
                item.Product!.Inventory.QuantityAvailable += item.Quantity;
            }

            // Clear old sale items
            await _productSaleRepository.RemoveProductSaleItemsAsync(sale.ProductSaleItems);
            sale.ProductSaleItems.Clear();

            decimal newTotalAmount = 0;
            var newSaleItems = new List<ProductSaleItem>();

            foreach (var item in request.ProductSaleItems)
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                if (product == null)
                    return new BaseResponse<bool> { Status = false, Message = $"Product {item.ProductId} not found." };

                if (item.ProductUnitType == ProductUnitType.SingleUnit && product.UnitPrice == null)
                    return new BaseResponse<bool> { Status = false, Message = $"Unit price not set for {product.Name}" };

                if (item.ProductUnitType == ProductUnitType.Pack && product.PricePerPack == null)
                    return new BaseResponse<bool> { Status = false, Message = $"Pack price not set for {product.Name}" };

                if (item.Quantity <= 0)
                    return new BaseResponse<bool> { Status = false, Message = $"Invalid quantity for {product.Name}" };

                decimal price;
                if (item.ProductUnitType == ProductUnitType.SingleUnit)
                {
                    price = product.UnitPrice!.Value * item.Quantity;
                }
                else
                {
                    if (item.Quantity % product.TotalItemInPack == 0)
                    {
                        int numberOfPacks = item.Quantity / product.TotalItemInPack;
                        price = product.PricePerPack!.Value * numberOfPacks;
                    }
                    else
                    {
                        decimal unitFromPack = product.PricePerPack!.Value / product.TotalItemInPack;
                        price = unitFromPack * item.Quantity;
                    }
                }

                if (product.Inventory.QuantityAvailable < item.Quantity)
                    return new BaseResponse<bool> { Status = false, Message = $"Not enough stock for {product.Name}" };

                // Deduct inventory
                product.Inventory.QuantityAvailable -= item.Quantity;

                newSaleItems.Add(new ProductSaleItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    ProductUnitType = item.ProductUnitType,
                    SubTotal = price
                });

                newTotalAmount += price;
            }

            sale.CustomerName = request.CustomerName;
            sale.PaymentMethod = request.PaymentMethod;
            sale.Discount = request.Discount ?? 0;
            sale.TotalAmount = newTotalAmount - (sale.Discount ?? 0);
            sale.ProductSaleItems = newSaleItems;

            var updated = await _productSaleRepository.UpdateAsync(sale);
            if (updated == false)
            {
                return new BaseResponse<bool> { Status = false, Message = "Failed to update the sale." };
            }

            return new BaseResponse<bool>
            {
                Status = true,
                Message = "Sale updated successfully.",
                Data = true
            };
        }


    }
}
