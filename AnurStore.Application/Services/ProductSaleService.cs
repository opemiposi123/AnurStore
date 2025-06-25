using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Common.Contracts;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using System.Text.Json;

namespace AnurStore.Application.Services
{
    public class ProductSaleService : IProductSaleService
    {
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;

        public ProductSaleService(IProductSaleRepository productSaleRepository, IProductRepository productRepository, IReceiptService receiptService, IProductService productService)
        {
            _productSaleRepository = productSaleRepository;
            _productRepository = productRepository;
            _receiptService = receiptService;
            _productService = productService;
        }

        public async Task<BaseResponse<byte[]>> AddProductSale(CreateProductSaleRequest request)
        {
            try
            {
                decimal totalAmount = 0;
                var saleItems = new List<ProductSaleItem>();
                var productNames = new Dictionary<string, string>();

                foreach (var item in request.ProductSaleItems)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);

                    if (product == null)
                    {
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Product with ID {item.ProductId} not found"
                        };
                    }

                    if (item.ProductUnitType == ProductUnitType.SingleUnit && product.UnitPrice == null)
                    {
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Product {product.Name} does not support unit pricing"
                        };
                    }

                    if ((item.ProductUnitType == ProductUnitType.Pack ||
                        item.ProductUnitType == ProductUnitType.HalfPack ||
                        item.ProductUnitType == ProductUnitType.QuarterPack) && product.PricePerPack == null)
                    {
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Product {product.Name} does not support pack-based pricing"
                        };
                    }

                    if (product.Inventory == null)
                    {
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"{product.Name} has not been stocked yet. Please check back later or contact inventory management."
                        };
                    }

                    if (product.TotalItemInPack <= 0)
                    {
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Invalid TotalItemInPack for product: {product.Name}"
                        };
                    }

                    decimal unitPriceFromPack = product.PricePerPack.HasValue && product.TotalItemInPack > 0
                        ? product.PricePerPack.Value / product.TotalItemInPack
                        : 0;

                    decimal subTotal = item.ProductUnitType switch
                    {
                        ProductUnitType.SingleUnit => product.UnitPrice!.Value * item.Quantity,
                        ProductUnitType.Pack => product.PricePerPack!.Value * item.Quantity,
                        ProductUnitType.HalfPack => unitPriceFromPack * product.TotalItemInPack * 0.5m * item.Quantity,
                        ProductUnitType.QuarterPack => unitPriceFromPack * product.TotalItemInPack * 0.25m * item.Quantity,
                        _ => 0
                    };

                    int totalUnitsToDeduct = item.ProductUnitType switch
                    {
                        ProductUnitType.Pack => item.Quantity * product.TotalItemInPack,
                        ProductUnitType.HalfPack => (int)(0.5 * product.TotalItemInPack * item.Quantity),
                        ProductUnitType.QuarterPack => (int)(0.25 * product.TotalItemInPack * item.Quantity),
                        ProductUnitType.SingleUnit => item.Quantity,
                        _ => 0
                    };

                    var inventory = product.Inventory;
                    int totalAvailableUnits = inventory.QuantityAvailable * product.TotalItemInPack;

                    if (totalAvailableUnits < totalUnitsToDeduct)
                    {
                        return new BaseResponse<byte[]>
                        {
                            Status = false,
                            Message = $"Insufficient stock for {product.Name}. Only {totalAvailableUnits} units available."
                        };
                    }

                    totalAvailableUnits -= totalUnitsToDeduct;
                    inventory.QuantityAvailable = totalAvailableUnits / product.TotalItemInPack;

                    await _productRepository.UpdateProduct(product);

                    saleItems.Add(new ProductSaleItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        ProductUnitType = item.ProductUnitType,
                        SubTotal = subTotal,
                        CreatedBy = item.CreatedBy
                    });

                    productNames[product.Id] = product.Name;
                    totalAmount += subTotal;
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
                        ProductName = productNames[si.ProductId],
                        Quantity = si.Quantity,
                        ProductUnitType = si.ProductUnitType,
                        SubTotal = si.SubTotal
                    }).ToList()
                };

                var (receiptDto, pdfBytes) = await _receiptService.GenerateFromProductSaleAsync(saleDto);


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




        //public async Task<BaseResponse<CreateProductSaleRequest>> PrepareSaleRequestAsync(CreateProductSaleViewModel viewModel)
        //{
        //    if (string.IsNullOrWhiteSpace(viewModel.SaleRequest.ProductSaleItemsJson))
        //    {
        //        return new BaseResponse<CreateProductSaleRequest>
        //        {
        //            Status = false,
        //            Message = "No product items found in request."
        //        };
        //    }

        //    try
        //    {
        //        viewModel.SaleRequest.ProductSaleItems = JsonSerializer.Deserialize<List<CreateProductSaleItemRequest>>(viewModel.SaleRequest.ProductSaleItemsJson) ?? new();

        //        return new BaseResponse<CreateProductSaleRequest>
        //        {
        //            Status = true,
        //            Message = "Request prepared successfully.",
        //            Data = viewModel.SaleRequest
        //        };
        //    }
        //    catch
        //    {
        //        return new BaseResponse<CreateProductSaleRequest>
        //        {
        //            Status = false,
        //            Message = "Invalid format for product sale items."
        //        };
        //    }
        //}



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


        public async Task<BaseResponse<List<ProductDto>>> GetTopFrequentlySoldProductsAsync(int days = 7)
        {
            try
            {
                var endDate = DateTime.Now.Date.AddDays(1).AddTicks(-1);
                var startDate = DateTime.Now.Date.AddDays(-7);

                var recentProductIds = await _productSaleRepository.GetTopSoldProductsRawAsync(startDate, endDate);

                var products = new List<ProductDto>();

                foreach (var productId in recentProductIds)
                {
                    var product = await _productRepository.GetProductById(productId);
                    if (product != null)
                    {
                        products.Add(new ProductDto
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            UnitPrice = product.UnitPrice,
                            PricePerPack = product.PricePerPack,
                            PackPriceMarkup = product.PackPriceMarkup,
                            TotalItemInPack = product.TotalItemInPack,
                            ProductImageUrl = product.ProductImageUrl,
                            CreatedBy = product.CreatedBy,
                            CreatedOn = product.CreatedOn,
                            CategoryName = product.Category?.Name ?? "N/A",
                            BrandName = product.Brand?.Name ?? "N/A",
                            Size = product.ProductSize?.Size ?? 0,
                            UnitName = product.ProductSize?.ProductUnit?.Name ?? "N/A"
                        });
                    }
                }
                return new BaseResponse<List<ProductDto>>
                {
                    Data = products,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ProductDto>>
                {
                    Message = $"Failed to retrieve products: {ex.Message}",
                    Status = false
                };
            }
        }



        public async Task<PagedResponse<List<ProductSaleDto>>> GetAllProductSalesPagedAsync(int pageNumber, int pageSize)
        {
            var productSales = await _productSaleRepository.GetProductSalesPagedAsync(pageNumber, pageSize);
            var totalRecords = await _productSaleRepository.GetTotalProductSalesCountAsync();

            if (!productSales.Any())
            {
                return new PagedResponse<List<ProductSaleDto>>
                {
                    Status = false,
                    Message = "No product sales found.",
                };
            }

            var pagedSales = productSales.Select(sale => new ProductSaleDto
            {
                Id = sale.Id,
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

            return new PagedResponse<List<ProductSaleDto>>
            {
                Data = pagedSales,
                Message = "Product sales retrieved successfully.",
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = true,
                TotalRecords = totalRecords
            };
        }



        public async Task<PagedResponse<List<ProductSaleDto>>> GetFilteredProductSalesPagedAsync(ProductSaleFilterRequest filter)
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

            return new PagedResponse<List<ProductSaleDto>>
            {
                Status = true,
                Message = "Sales retrieved successfully.",
                Data = pagedSales,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords
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


        public async Task<BaseResponse<bool>> UpdateProductSaleAsync(string productSaleId, UpdateProductSaleRequest request)
        {
            try
            {
                var sale = await _productSaleRepository.GetProductSaleByIdAsync(productSaleId);
                if (sale == null)
                    return new BaseResponse<bool> { Status = false, Message = "Sale not found." };

                if (sale.IsDeleted)
                    return new BaseResponse<bool> { Status = false, Message = "Cannot update a cancelled sale." };

                // === Revert Old Inventory ===
                foreach (var oldItem in sale.ProductSaleItems)
                {
                    var product = await _productRepository.GetProductById(oldItem.ProductId);
                    if (product == null) continue;

                    int unitsToAddBack = oldItem.ProductUnitType switch
                    {
                        ProductUnitType.Pack => oldItem.Quantity * product.TotalItemInPack,
                        ProductUnitType.HalfPack => (int)(0.5 * product.TotalItemInPack * oldItem.Quantity),
                        ProductUnitType.QuarterPack => (int)(0.25 * product.TotalItemInPack * oldItem.Quantity),
                        ProductUnitType.SingleUnit => oldItem.Quantity,
                        _ => 0
                    };

                    int currentUnits = product.Inventory.QuantityAvailable * product.TotalItemInPack;
                    int updatedUnits = currentUnits + unitsToAddBack;
                    product.Inventory.QuantityAvailable = updatedUnits / product.TotalItemInPack;

                    await _productRepository.UpdateProduct(product);
                }

                // Clear old sale items
                await _productSaleRepository.RemoveProductSaleItemsAsync(sale.ProductSaleItems);
                sale.ProductSaleItems.Clear();

                // === Recalculate New Items ===
                decimal totalAmount = 0;
                var newSaleItems = new List<ProductSaleItem>();

                foreach (var item in request.ProductSaleItems)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);
                    if (product == null)
                        return new BaseResponse<bool> { Status = false, Message = $"Product {item.ProductId} not found." };

                    // Pricing Validation
                    if (item.ProductUnitType == ProductUnitType.SingleUnit && product.UnitPrice == null)
                        return new BaseResponse<bool> { Status = false, Message = $"Product {product.Name} does not support unit pricing" };

                    if ((item.ProductUnitType == ProductUnitType.Pack ||
                        item.ProductUnitType == ProductUnitType.HalfPack ||
                        item.ProductUnitType == ProductUnitType.QuarterPack) && product.PricePerPack == null)
                        return new BaseResponse<bool> { Status = false, Message = $"Product {product.Name} does not support pack pricing" };

                    if (item.Quantity <= 0)
                        return new BaseResponse<bool> { Status = false, Message = $"Invalid quantity for {product.Name}" };

                    // Calculate unit price from pack
                    decimal unitPriceFromPack = product.PricePerPack.HasValue && product.TotalItemInPack > 0
                        ? product.PricePerPack.Value / product.TotalItemInPack
                        : 0;

                    decimal subTotal = item.ProductUnitType switch
                    {
                        ProductUnitType.SingleUnit => product.UnitPrice!.Value * item.Quantity,
                        ProductUnitType.Pack => product.PricePerPack!.Value * item.Quantity,
                        ProductUnitType.HalfPack => unitPriceFromPack * product.TotalItemInPack * 0.5m * item.Quantity,
                        ProductUnitType.QuarterPack => unitPriceFromPack * product.TotalItemInPack * 0.25m * item.Quantity,
                        _ => 0
                    };

                    int unitsToDeduct = item.ProductUnitType switch
                    {
                        ProductUnitType.Pack => item.Quantity * product.TotalItemInPack,
                        ProductUnitType.HalfPack => (int)(0.5 * product.TotalItemInPack * item.Quantity),
                        ProductUnitType.QuarterPack => (int)(0.25 * product.TotalItemInPack * item.Quantity),
                        ProductUnitType.SingleUnit => item.Quantity,
                        _ => 0
                    };

                    int availableUnits = product.Inventory.QuantityAvailable * product.TotalItemInPack;

                    if (availableUnits < unitsToDeduct)
                    {
                        return new BaseResponse<bool>
                        {
                            Status = false,
                            Message = $"Insufficient stock for {product.Name}. Only {availableUnits} units available."
                        };
                    }

                    // Deduct from inventory
                    availableUnits -= unitsToDeduct;
                    product.Inventory.QuantityAvailable = availableUnits / product.TotalItemInPack;

                    await _productRepository.UpdateProduct(product);

                    newSaleItems.Add(new ProductSaleItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        ProductUnitType = item.ProductUnitType,
                        SubTotal = subTotal,
                        CreatedBy = sale.CreatedBy
                    });

                    totalAmount += subTotal;
                }

                // === Update Sale ===
                decimal discount = request.Discount ?? 0;
                sale.CustomerName = request.CustomerName;
                sale.PaymentMethod = request.PaymentMethod;
                sale.Discount = discount;
                sale.TotalAmount = totalAmount - discount;
                sale.ProductSaleItems = newSaleItems;
                sale.LastModifiedOn = DateTime.Now;

                var updated = await _productSaleRepository.UpdateAsync(sale);
                if (!updated)
                {
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Failed to update sale."
                    };
                }

                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = "Sale updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"Error occurred while updating sale: {ex.Message}"
                };
            }
        }



    }
}
