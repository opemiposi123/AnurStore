using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public Task<BaseResponse<bool>> UpdateProduct(string productId, UpdateProductRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ProductDto>> GetProduct(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
