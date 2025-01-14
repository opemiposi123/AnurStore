using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AnurStore.Application.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IProductSizeRepository productSizeRepository,ILogger<CategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _productSizeRepository = productSizeRepository;
            _logger = logger; 
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> CreateProductAsync(CreateProductRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting CreateProductAsync method.");

            if (request == null)
            {
                _logger.LogWarning("CreateProduct request is null.");
                return new BaseResponse<string>
                {
                    Message = "Request cannot be null",
                    Status = false,
                };
            }

            try
            {
                _logger.LogInformation("Checking if Product with name {ProductName} exists.", request.Name);
                if (await _productRepository.Exist(request.Name))
                {
                    _logger.LogWarning("Product with name {ProductName} already exists.", request.Name);
                    return new BaseResponse<string>
                    {
                        Status = false,
                        Message = "Product name already exists"
                    };
                }

                string productImageUrl = null;
                if (request.ProductImage != null && request.ProductImage.Length > 0)
                {
                    productImageUrl = await SaveFileAsync(request.ProductImage);
                }

                var product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    BarCode = request.BarCode,
                    BrandId = request.BrandId,
                    CategoryId = request.CategoryId,
                    UnitPrice = request.UnitPrice,
                    PricePerPack = request.PricePerPack,
                    UnitPriceMarkup = request.UnitPriceMarkup,
                    PackPriceMarkup = request.PackPriceMarkup,
                    TotalItemInPack = request.TotalItemInPack,
                    ProductImageUrl = productImageUrl,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now
                };

                var productSize = new ProductSize
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductUnitId = request.UnitId,
                    ProductId = product.Id,
                    Size = request.ProductSize,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now
                };

                _logger.LogInformation("Creating Product and its size for {ProductName}.", request.Name);
                var productId = await _productRepository.CreateProductWithSizeAsync(product, productSize);

                _logger.LogInformation("Product {ProductName} created successfully with Id {ProductId}.", request.Name, productId);
                return new BaseResponse<string>
                {
                    Message = "Product created successfully",
                    Status = true,
                    Data = productId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Product.");
                return new BaseResponse<string>
                {
                    Message = $"Failed to create Product: {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploads = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            var filePath = Path.Combine(uploads, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/images/{file.FileName}";
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllProduct()
        {
            _logger.LogInformation("Starting GetAllProduct method.");
            try
            {
                var report = await _productRepository.GetAllProduct();
                var ProductDtos = report.Where(x => !x.IsDeleted).Select(r => new ProductDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    CategoryName = r.Category.Name,
                    BrandName = r.Brand.Name,
                    UnitPrice = r.UnitPrice,
                    PricePerPack = r.PricePerPack,
                    TotalItemInPack = r.TotalItemInPack,
                    ProductImageUrl = r.ProductImageUrl,
                    Size = r.ProductSize.Size,
                    UnitName = r.ProductSize.ProductUnit.Name,
                    Description = r.Description
                }).ToList();

                _logger.LogInformation("Successfully retrieved categories.");
                return new BaseResponse<IEnumerable<ProductDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = ProductDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return new BaseResponse<IEnumerable<ProductDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteProduct(string productId)
        {
            try
            {
                var Product = await _productRepository.GetProductById(productId);
                if (Product == null)
                {
                    _logger.LogWarning($"Product with Id {productId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Product not found",
                        Status = false,
                    };
                }

                Product.IsDeleted = true;

                var result = await _productRepository.UpdateProduct(Product);

                if (result)
                {
                    _logger.LogInformation($"Product with Id {productId} deleted successfully.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Product deleted successfuly",
                        Status = false,
                    };
                }
                else
                {
                    _logger.LogError($"Failed to delete Product with Id {productId}.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Fail to delete Product",
                        Status = false,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Product with Id {ProductId}.", productId);
                return new BaseResponse<bool>
                {
                    Message = $"An error occured {ex.Message}",
                    Status = false,
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateProduct(string productId, UpdateProductRequest request)
        {
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
            _logger.LogInformation("Starting UpdateProduct method for ProductId: {ProductId}.", productId);

            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                {
                    _logger.LogWarning("ProductId is null or empty.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "ProductId cannot be null or empty",
                    };
                }

                if (request == null)
                {
                    _logger.LogWarning("UpdateProduct request is null.");
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Request cannot be null",
                    };
                }

                // Retrieve the existing product
                var product = await _productRepository.GetProductById(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product with Id {ProductId} not found.", productId);
                    return new BaseResponse<bool>
                    {
                        Status = false,
                        Message = "Product not found",
                    };
                }

                // Update product properties
                product.Name = request.Name ?? product.Name;
                product.Description = request.Description ?? product.Description;
                product.BarCode = request.BarCode ?? product.BarCode;
                product.BrandId = request.BrandId ?? product.BrandId;
                product.CategoryId = request.CategoryId ?? product.CategoryId;
                product.UnitPrice = request.UnitPrice;
                product.PricePerPack = request.PricePerPack;
                product.UnitPriceMarkup = request.UnitPriceMarkup;
                product.PackPriceMarkup = request.PackPriceMarkup;
                product.TotalItemInPack = request.TotalItemInPack;
                product.LastModifiedBy = userName;
                product.LastModifiedOn = DateTime.Now;

                // Update product image if a new one is provided
                if (request.ProductImage != null && request.ProductImage.Length > 0)
                {
                    var newImageUrl = await SaveFileAsync(request.ProductImage);
                    product.ProductImageUrl = newImageUrl;
                }

                // Update product size if provided
                if (request.ProductSize != null)
                {
                    var existingSize = await _productRepository.GetProductSizeByProductIdAsync(productId);
                    if (existingSize != null)
                    {
                        existingSize.Size = request.ProductSize;
                        existingSize.ProductUnitId = request.UnitId;
                        existingSize.LastModifiedBy = userName;
                        existingSize.LastModifiedOn = DateTime.Now;

                        await _productSizeRepository.UpdateProductSize(existingSize);
                    }
                    else
                    {
                        var newSize = new ProductSize
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = product.Id,
                            ProductUnitId = request.UnitId,
                            Size = request.ProductSize,
                            CreatedBy = userName,
                            CreatedOn = DateTime.Now
                        };

                        await _productSizeRepository.CreateProductSize(newSize);
                    }
                }

                // Update the product
                await _productRepository.UpdateProduct(product);

                _logger.LogInformation("Product with Id {ProductId} updated successfully.", productId);
                return new BaseResponse<bool>
                {
                    Status = true,
                    Message = "Product updated successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Product with Id {ProductId}.", productId);
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = $"Failed to update Product: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<ProductDto>> GetProductDetails(string productId)
        {
            _logger.LogInformation("Starting GetProduct method for Id {ProductId}.", productId);
            try
            {
                var reportType = await _productRepository.GetProductById(productId);
                if (reportType == null)
                {
                    _logger.LogWarning("Product with Id {ProductId} not found.", productId);
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Product not found",
                        Status = false
                    };
                }
                var ProductDto = new ProductDto
                {
                    Id = productId,
                    Name = reportType.Name,
                    Description = reportType.Description,
                    CategoryName = reportType.Category.Name,
                    BrandName = reportType.Brand.Name,
                    UnitPrice = reportType.UnitPrice,
                    PricePerPack = reportType.PricePerPack,
                    TotalItemInPack = reportType.TotalItemInPack,
                    ProductImageUrl = reportType.ProductImageUrl,
                    Size = reportType.ProductSize.Size,
                    UnitName = reportType.ProductSize.ProductUnit.Name,
                    CreatedBy = reportType.CreatedBy,
                    CreatedOn = reportType.CreatedOn,

                };

                _logger.LogInformation("Successfully retrieved Product with Id {ProductId}.", productId);
                return new BaseResponse<ProductDto>
                {
                    Data = ProductDto,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Product with Id {ProductId}.", productId);
                return new BaseResponse<ProductDto>
                {
                    Message = $"Failed to retrieve Product: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetProductSelectList()
        {
            var productsResponse = await GetAllProduct();

            if (productsResponse.Status && productsResponse.Data != null)
            {
                var productList = productsResponse.Data.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(), 
                    Text = d.Name 
                });

                return productList;
            }
            return Enumerable.Empty<SelectListItem>();
        }
    }
}