using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.Helper;
using AnurStore.Application.Pagination;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace AnurStore.Application.Services
{
    public class ProductPurchaseService : IProductPurchaseService
    {
        private readonly IInventoryRepository _inventoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductPurchaseRepository _productpurchaseRepo;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProductPurchaseService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductPurchaseService(IInventoryRepository inventoryRepo,
            IProductRepository productRepo,
            IProductPurchaseRepository productPurchaseRepo,
            IProductService productService,
            ILogger<ProductPurchaseService> logger,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
             UserManager<User> userManager)
        {
            _inventoryRepo = inventoryRepo;
            _productRepo = productRepo;
            _productpurchaseRepo = productPurchaseRepo;
            _productService = productService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> PurchaseProductsAsync(CreateProductPurchaseRequest request, string userName)
        {
            _logger.LogInformation("Starting product purchase transaction. SupplierId: {SupplierId}, Batch: {Batch}",
                request.SupplierId, request.Batch);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var productPurchase = new ProductPurchase
                {
                    Batch = request.Batch,
                    SupplierId = request.SupplierId,
                    Discount = request.Discount,
                    Total = request.Total,
                    PurchaseDate = request.PurchaseDate,
                    IsAddedToInventory = request.IsAddedToInventory,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now,
                    PurchaseItems = []
                };

                var inventoryToAdd = new List<Inventory>();
                var productToUpdate = new List<Product>();
                var items = request.PurchaseItems.ToList();

                // Fetch all inventories and products sequentially to avoid concurrency issues
                var inventories = new List<Inventory?>();
                var products = new List<Product?>();

                foreach (var i in items)
                {
                    // Await each call before starting the next
                    var inventory = await _inventoryRepo.GetByProductAndBatchAsync(i.ProductId, request.Batch);
                    inventories.Add(inventory);
                    var product = await _productRepo.GetProductById(i.ProductId);
                    products.Add(product);
                }

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    _logger.LogInformation("Processing item for ProductId: {ProductId}, Qty: {Quantity}, Rate: {Rate}",
                        item.ProductId, item.Quantity, item.Rate);

                    var purchaseItem = new ProductPurchaseItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Rate = item.Rate,
                        TotalCost = item.TotalCost,
                        CreatedBy = userName,
                        CreatedOn = DateTime.Now
                    };

                    productPurchase.PurchaseItems.Add(purchaseItem);

                    if (request.IsAddedToInventory)
                    {
                        var product = await _productRepo.GetProductById(item.ProductId);

                        if (product == null)
                            throw new Exception($"Product with ID {item.ProductId} not found.");

                        int totalPieces = item.Quantity * product.TotalItemInPack;

                        var existingInventory = inventories[i];
                        if (existingInventory != null)
                        {
                            existingInventory.TotalPiecesAvailable += totalPieces;
                            existingInventory.StockDate = DateTime.Now;
                        }
                        else
                        {
                            inventoryToAdd.Add(new Inventory
                            {
                                ProductId = item.ProductId,
                                TotalPiecesAvailable = totalPieces,
                                StockDate = DateTime.Now,
                                BatchNumber = request.Batch,
                                ExpirationDate = item.ExpirationDate,
                                StockStatus = StockStatus.InStock,
                                Remark = "Restocked",
                                CreatedBy = userName,
                                CreatedOn = DateTime.Now
                            });
                        }
                    }


                    var productEntity = products[i];
                    if (productEntity != null)
                    {
                        var newPackPrice = PricingCalculator.CalculatePackSellingPrice(item.Rate, productEntity.PackPriceMarkup);
                        var unitsPerPack = productEntity.TotalItemInPack;

                        var newUnitPrice = unitsPerPack > 0
                            ? PricingCalculator.CalculateUnitSellingPrice(newPackPrice, unitsPerPack)
                            : 0;

                        productEntity.PricePerPack = newPackPrice;
                        productEntity.UnitPrice = newUnitPrice;

                        productToUpdate.Add(productEntity);
                    }
                }

                await _productpurchaseRepo.PurchaseProductAsync(productPurchase);

                foreach (var inventory in inventoryToAdd)
                {
                    await _inventoryRepo.AddAsync(inventory);
                }

                foreach (var inventory in inventories.Where(x => x != null))
                {
                    await _inventoryRepo.UpdateAsync(inventory!);
                }

                foreach (var product in productToUpdate)
                {
                    await _productRepo.UpdateProduct(product);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Product purchase completed. PurchaseId: {PurchaseId}", productPurchase.Id);

                return new BaseResponse<string>
                {
                    Status = true,
                    Message = "Product purchase completed successfully",
                    Data = productPurchase.Id
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Purchase failed. SupplierId: {SupplierId}, Batch: {Batch}", request.SupplierId, request.Batch);

                return new BaseResponse<string>
                {
                    Status = false,
                    Message = $"Purchase failed: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<bool>> DeletePurchaseAsync(string purchaseId)
        {
            try
            {
                var purchase = await _productpurchaseRepo.GetByIdAsync(purchaseId);
                if (purchase == null)
                {
                    _logger.LogWarning($"Purchase with Id {purchaseId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Brand not found",
                        Status = false,
                    };
                }

                purchase.IsDeleted = true;

                var result = await _productpurchaseRepo.UpdateAsync(purchase);

                if (result)
                {
                    _logger.LogInformation($"Purchase with Id {purchaseId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Purchase deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete Purchase with Id {purchaseId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete Purchase",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Purchase with Id {BrandId}.", purchaseId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetAllPurchasesAsync()
        {
            _logger.LogInformation("Starting GetAllProductPurchase method.");
            try
            {
                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                    {
                        Status = false,
                        Message = "User context not found."
                    };
                }

                var user = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                    {
                        Status = false,
                        Message = "User not found."
                    };
                }

                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                var username = userPrincipal?.Identity?.Name;
                var response = await _productpurchaseRepo.GetAllAsync(username);
                var productPurchaseeDtos = response.Where(x => !x.IsDeleted).Select(r => new ProductPurchaseDto
                {
                    Id = r.Id,
                    SupplierName = r.Supplier.Name,
                    Total = r.Total,
                    Discount = r.Discount,
                    PurchaseDate = r.PurchaseDate,
                    Batch = r.Batch,
                    CreatedBy = r.CreatedBy
                }).ToList();

                _logger.LogInformation("Successfully retrieved product purchaseds.");
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = productPurchaseeDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product purchased.");
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<ProductPurchaseDto>> GetPurchaseDetailsAsync(string purchaseId)
        {
            var purchase = await _productpurchaseRepo.GetByIdAsync(purchaseId);

            if (purchase == null)
            {
                return new BaseResponse<ProductPurchaseDto>
                {
                    Status = false,
                    Message = "Purchase not found"
                };
            }

            var dto = new ProductPurchaseDto
            {
                Id = purchase.Id,
                Batch = purchase.Batch,
                Total = purchase.Total,
                Discount = purchase.Discount,
                PurchaseDate = purchase.PurchaseDate,
                SupplierName = purchase.Supplier.Name,
                IsAddedToInventory = purchase.IsAddedToInventory,
                CreatedBy = purchase.CreatedBy,
                PurchaseItems = purchase.PurchaseItems.Select(item => new ProductPurchaseItemDto
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Rate = item.Rate,
                    TotalCost = item.TotalCost
                }).ToList()
            };

            return new BaseResponse<ProductPurchaseDto>
            {
                Status = true,
                Data = dto,
                Message = "Purchase details retrieved successfully"
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesBySupplierAsync(string supplierId)
        {
            var purchases = await _productpurchaseRepo.GetBySupplierIdAsync(supplierId);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found for this supplier"
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                SupplierName = p.Supplier.Name,
                IsAddedToInventory = p.IsAddedToInventory
            }).ToList();

            return new BaseResponse<IEnumerable<ProductPurchaseDto>>
            {
                Status = true,
                Data = dtos,
                Message = "Purchases retrieved successfully"
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var purchases = await _productpurchaseRepo.GetByDateRangeAsync(startDate, endDate);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found for the selected date range."
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                IsAddedToInventory = p.IsAddedToInventory,
                SupplierName = p.Supplier.Name
            }).ToList();

            return new BaseResponse<IEnumerable<ProductPurchaseDto>>
            {
                Status = true,
                Message = "Purchases retrieved successfully",
                Data = dtos
            };
        }

        public async Task<BaseResponse<IEnumerable<ProductPurchaseDto>>> GetPurchasesByProductAsync(string productId)
        {
            var purchases = await _productpurchaseRepo.GetPurchasesByProductAsync(productId);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<IEnumerable<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found for this product",
                    Data = Enumerable.Empty<ProductPurchaseDto>()
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                IsAddedToInventory = p.IsAddedToInventory,
                SupplierName = p.Supplier?.Name ?? "N/A"
            }).ToList();

            return new BaseResponse<IEnumerable<ProductPurchaseDto>>
            {
                Status = true,
                Message = "Purchases retrieved successfully",
                Data = dtos
            };
        }

        public async Task<BaseResponse<byte[]>> ExportPurchasesToExcelAsync(PurchaseExportRequest request)
        {
            var purchases = await _productpurchaseRepo.GetAllWithDetailsAsync();

            // Apply filters
            if (request.StartDate.HasValue)
                purchases = purchases.Where(p => p.PurchaseDate >= request.StartDate.Value).ToList();

            if (request.EndDate.HasValue)
                purchases = purchases.Where(p => p.PurchaseDate <= request.EndDate.Value).ToList();

            if (!string.IsNullOrEmpty(request.SupplierId))
                purchases = purchases.Where(p => p.SupplierId == request.SupplierId).ToList();

            if (!string.IsNullOrEmpty(request.ProductId))
                purchases = purchases
                    .Where(p => p.PurchaseItems.Any(i => i.ProductId == request.ProductId))
                    .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Purchases");

            // Headers
            worksheet.Cells[1, 1].Value = "Purchase ID";
            worksheet.Cells[1, 2].Value = "Supplier";
            worksheet.Cells[1, 3].Value = "Batch";
            worksheet.Cells[1, 4].Value = "Total";
            worksheet.Cells[1, 5].Value = "Discount";
            worksheet.Cells[1, 6].Value = "Date";
            worksheet.Cells[1, 7].Value = "Product(s)";

            int row = 2;
            foreach (var p in purchases)
            {
                worksheet.Cells[row, 1].Value = p.Id;
                worksheet.Cells[row, 2].Value = p.Supplier?.Name ?? "N/A";
                worksheet.Cells[row, 3].Value = p.Batch;
                worksheet.Cells[row, 4].Value = p.Total;
                worksheet.Cells[row, 5].Value = p.Discount;
                worksheet.Cells[row, 6].Value = p.PurchaseDate.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 7].Value = string.Join(", ", p.PurchaseItems.Select(i => i.Product.Name));

                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = await package.GetAsByteArrayAsync();

            return new BaseResponse<byte[]>
            {
                Status = true,
                Message = "Excel file generated successfully",
                Data = fileBytes
            };
        }

        public async Task<BaseResponse<PaginatedResult<ProductPurchaseDto>>> GetPurchasesPagedAsync(PurchaseFilterRequest filter)
        {
            var (purchases, totalCount) = await _productpurchaseRepo.GetPagedPurchasesAsync(filter);

            if (purchases == null || !purchases.Any())
            {
                return new BaseResponse<PaginatedResult<ProductPurchaseDto>>
                {
                    Status = false,
                    Message = "No purchases found",
                    Data = new PaginatedResult<ProductPurchaseDto>
                    {
                        Items = new List<ProductPurchaseDto>(),
                        TotalCount = 0,
                        PageNumber = filter.PageNumber,
                        PageSize = filter.PageSize
                    }
                };
            }

            var dtos = purchases.Select(p => new ProductPurchaseDto
            {
                Id = p.Id,
                Batch = p.Batch,
                Total = p.Total,
                Discount = p.Discount,
                PurchaseDate = p.PurchaseDate,
                IsAddedToInventory = p.IsAddedToInventory,
                SupplierName = p.Supplier?.Name ?? "N/A"
            }).ToList();

            return new BaseResponse<PaginatedResult<ProductPurchaseDto>>
            {
                Status = true,
                Message = "Paged purchases retrieved successfully",
                Data = new PaginatedResult<ProductPurchaseDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                }
            };
        }

        private class PurchaseChanges
        {
            public List<Inventory> InventoriesToAdd { get; } = new();
            public List<Inventory> InventoriesToUpdate { get; } = new();
            public List<Product> ProductsToUpdate { get; } = new();
        }

        public async Task<BaseResponse<string>> UpdatePurchaseProductsAsync(CreateProductPurchaseRequest request, string userName)
        {
            _logger.LogInformation("Starting product purchase transaction. SupplierId: {SupplierId}, Batch: {Batch}",
                request.SupplierId, request.Batch);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var productPurchase = CreateProductPurchase(request, userName);

                var productIds = request.PurchaseItems.Select(x => x.ProductId).Distinct().ToList();

                var (products, inventories) = await FetchRequiredDataAsync(productIds, request.Batch);

                var changes = await ProcessPurchaseItemsAsync(request, userName, productPurchase, products, inventories);

                await ApplyChangesAsync(productPurchase, changes);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Product purchase completed. PurchaseId: {PurchaseId}", productPurchase.Id);

                return new BaseResponse<string>
                {
                    Status = true,
                    Message = "Product purchase completed successfully",
                    Data = productPurchase.Id
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Purchase failed. SupplierId: {SupplierId}, Batch: {Batch}", request.SupplierId, request.Batch);

                return new BaseResponse<string>
                {
                    Status = false,
                    Message = $"Purchase failed: {ex.Message}"
                };
            }
        }

        private ProductPurchase CreateProductPurchase(CreateProductPurchaseRequest request, string userName)
        {
            return new ProductPurchase
            {
                Batch = request.Batch,
                SupplierId = request.SupplierId,
                Discount = request.Discount,
                Total = request.Total,
                PurchaseDate = request.PurchaseDate,
                IsAddedToInventory = request.IsAddedToInventory,
                CreatedBy = userName,
                CreatedOn = DateTime.UtcNow,
                PurchaseItems = []
            };
        }

        private async Task<(Dictionary<string, Product> products, Dictionary<string, Inventory?> inventories)>
           FetchRequiredDataAsync(List<string> productIds, string batch)
        {
            var productsTask = _productRepo.GetProductsByIdsAsync(productIds);
            var inventoriesTask = _inventoryRepo.GetByProductsAndBatchAsync(productIds, batch);

            await Task.WhenAll(productsTask, inventoriesTask);

            var products = (await productsTask).ToDictionary(p => p.Id);
            var inventories = (await inventoriesTask).ToDictionary(i => i.ProductId, i => (Inventory?)i);

            // Ensure all requested products are found
            foreach (var id in productIds)
            {
                if (!products.ContainsKey(id))
                    throw new Exception($"Product with ID '{id}' not found.");
            }

            return (products, inventories);
        }


        private async Task<PurchaseChanges> ProcessPurchaseItemsAsync(
               CreateProductPurchaseRequest request,
               string userName,
               ProductPurchase productPurchase,
               Dictionary<string, Product> products,
               Dictionary<string, Inventory?> inventories)
        {
            var changes = new PurchaseChanges();
            var now = DateTime.UtcNow;

            foreach (var item in request.PurchaseItems)
            {
                _logger.LogInformation("Processing item: ProductId={ProductId}, Qty={Quantity}, Rate={Rate}",
                    item.ProductId, item.Quantity, item.Rate);

                // Add to purchase item list
                var purchaseItem = new ProductPurchaseItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Rate = item.Rate,
                    TotalCost = item.TotalCost,
                    CreatedBy = userName,
                    CreatedOn = now
                };
                productPurchase.PurchaseItems.Add(purchaseItem);

                var product = products[item.ProductId];

                if (request.IsAddedToInventory)
                {
                    ProcessInventoryUpdate(item, product, request, userName, inventories, changes, now);
                }

                UpdateProductPrices(item, product, changes);
            }

            return changes;
        }


        private void ProcessInventoryUpdate(
               CreateProductPurchaseItemRequest item,
               Product product,
               CreateProductPurchaseRequest request,
               string userName,
               Dictionary<string, Inventory?> inventories,
               PurchaseChanges changes,
               DateTime now)
        {
            int totalPieces = item.Quantity * product.TotalItemInPack;

            if (inventories.TryGetValue(item.ProductId, out var existingInventory) && existingInventory != null)
            {
                existingInventory.TotalPiecesAvailable += totalPieces;
                existingInventory.StockDate = now;
                changes.InventoriesToUpdate.Add(existingInventory);
            }
            else
            {
                changes.InventoriesToAdd.Add(new Inventory
                {
                    ProductId = item.ProductId,
                    TotalPiecesAvailable = totalPieces,
                    StockDate = now,
                    BatchNumber = request.Batch,
                    ExpirationDate = item.ExpirationDate,
                    StockStatus = StockStatus.InStock,
                    Remark = "Restocked",
                    CreatedBy = userName,
                    CreatedOn = now
                });
            }
        }


        private void UpdateProductPrices(CreateProductPurchaseItemRequest item, Product product, PurchaseChanges changes)
        {
            var newPackPrice = PricingCalculator.CalculatePackSellingPrice(item.Rate, product.PackPriceMarkup);
            var unitPrice = product.TotalItemInPack > 0
                ? PricingCalculator.CalculateUnitSellingPrice(newPackPrice, product.TotalItemInPack)
                : 0;

            product.PricePerPack = newPackPrice;
            product.UnitPrice = unitPrice;

            changes.ProductsToUpdate.Add(product);
        }


        private async Task ApplyChangesAsync(ProductPurchase purchase, PurchaseChanges changes)
        {
            await _productpurchaseRepo.PurchaseProductAsync(purchase);

            if (changes.InventoriesToAdd.Any())
                await _inventoryRepo.AddRangeAsync(changes.InventoriesToAdd);

            if (changes.InventoriesToUpdate.Any())
                await _inventoryRepo.UpdateRangeAsync(changes.InventoriesToUpdate);

            if (changes.ProductsToUpdate.Any())
                await _productRepo.UpdateRangeAsync(changes.ProductsToUpdate);
        }
    }

}
