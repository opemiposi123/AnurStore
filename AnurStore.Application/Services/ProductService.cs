using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.Abstractions.Services;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using ClosedXML.Excel;
namespace AnurStore.Application.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductUnitRepository _productUnitRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IProductUnitService _productUnitService;
        public ProductService(IProductRepository productRepository,
            ICategoryService categoryService,
            IProductSizeRepository productSizeRepository,
            ILogger<ProductService> logger,
            IProductUnitService productUnitService,
            IBrandService brandService,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IProductUnitRepository productUnitRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _productSizeRepository = productSizeRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _categoryService = categoryService;
            _brandService = brandService;
            _productUnitService = productUnitService;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _logger = logger;
            _productUnitRepository = productUnitRepository;
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

        public async Task<IEnumerable<ProductDto>> SearchProductsByNameAsync(string query)
        {
            var products = await _productRepository.SearchProductsByNameAsync(query);
            return products;
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
                    BrandName = r.Brand.Name ?? "Unknown",
                    UnitPrice = r.UnitPrice,
                    PricePerPack = r.PricePerPack,
                    TotalItemInPack = r.TotalItemInPack,
                    ProductImageUrl = r.ProductImageUrl,
                    Description = r.Description,
                    SizeWithUnit = $"{r.ProductSize.Size}{r.ProductSize.ProductUnit.Name}"
                }).ToList();

                _logger.LogInformation("Successfully retrieved products.");
                return new BaseResponse<IEnumerable<ProductDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = ProductDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products.");
                return new BaseResponse<IEnumerable<ProductDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllDisplayProducts()
        {
            _logger.LogInformation("Starting GetAllProduct method.");
            try
            {
                var report = await _productRepository.GetAllProduct();
                var ProductDtos = report.Where(x => !x.IsDeleted).Select(r => new ProductDto
                {
                    Id = r.Id,
                    Name = $"{r.Name} - {r.ProductSize.Size} {r.ProductSize.ProductUnit.Name} ({r.Brand.Name})",
                    CategoryName = r.Category.Name,
                    BrandName = r.Brand.Name ?? "Unknown",
                    UnitPrice = r.UnitPrice,
                    PricePerPack = r.PricePerPack,
                    TotalItemInPack = r.TotalItemInPack,
                    ProductImageUrl = r.ProductImageUrl,
                    Description = r.Description,
                    SizeWithUnit = $"{r.ProductSize.Size}{r.ProductSize.ProductUnit.Name}"
                }).ToList().Take(9);

                _logger.LogInformation("Successfully retrieved products.");
                return new BaseResponse<IEnumerable<ProductDto>>
                {
                    Message = "Record Found Successfully",
                    Status = true,
                    Data = ProductDtos,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products.");
                return new BaseResponse<IEnumerable<ProductDto>>
                {
                    Status = false,
                    Message = "Failed",
                };
            }
        }

        public async Task<List<ProductSearchResultDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.GetAllProduct();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = products
                    .Where(p => p.Name != null && p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return products.Select(p => new ProductSearchResultDto
            {
                Id = p.Id,
                Name = $"{p.Name} - {p.ProductSize} {p.ProductSize.ProductUnit.Name} ({p.Brand})",
                PricePerPack = p.PricePerPack,
                ImageUrl = p.ProductImageUrl
            }).ToList();

        }

        public async Task<BaseResponse<bool>> DeleteProduct(string productId)
        {
            try
            {
                var product = await _productRepository.GetProductById(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with Id {productId} not found.");
                    return new BaseResponse<bool>
                    {
                        Message = $"Product not found",
                        Status = false,
                    };
                }

                product.IsDeleted = true;

                await _productRepository.UpdateProduct(product);
                _logger.LogInformation($"Product with Id {productId} deleted successfully.");
                return new BaseResponse<bool>
                {
                    Message = $"Product deleted successfuly",
                    Status = false,
                };

                //if (result)
                //{
                //    _logger.LogInformation($"Product with Id {productId} deleted successfully.");
                //    return new BaseResponse<bool>
                //    {
                //        Message = $"Product deleted successfuly",
                //        Status = false,
                //    };
                //}
                //else
                //{
                //    _logger.LogError($"Failed to delete Product with Id {productId}.");
                //    return new BaseResponse<bool>
                //    {
                //        Message = $"Fail to delete Product",
                //        Status = false,
                //    };
                //}
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
                product.PackPriceMarkup = request.PackPriceMarkup;
                product.TotalItemInPack = request.TotalItemInPack;
                product.LastModifiedBy = userName;
                product.LastModifiedOn = DateTime.Now;

                if (request.ProductImage != null && request.ProductImage.Length > 0)
                {
                    var newImageUrl = await SaveFileAsync(request.ProductImage);
                    product.ProductImageUrl = newImageUrl;
                }

                if (request.ProductSize != null)
                {
                    var existingSize = await _productRepository.GetProductSizeByProductIdAsync(productId, noTracking: true);
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
                var product = await _productRepository.GetProductById(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product with Id {ProductId} not found.", productId);
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Product not found",
                        Status = false
                    };
                }
                var productDto = new ProductDto
                {
                    Id = productId,
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    PricePerPack = product.PricePerPack,
                    PackPriceMarkup = product.PackPriceMarkup,
                    TotalItemInPack = product.TotalItemInPack,
                    ProductImageUrl = product.ProductImageUrl,
                    BarCode = product.BarCode,
                    CreatedBy = product.CreatedBy,
                    CreatedOn = product.CreatedOn,
                    CategoryName = product.Category?.Name ?? "N/A",
                    BrandName = product.Brand?.Name ?? "N/A",
                    Size = product.ProductSize?.Size ?? 0,
                    UnitName = product.ProductSize?.ProductUnit?.Name ?? "N/A",


                };

                _logger.LogInformation("Successfully retrieved Product with Id {ProductId}.", productId);
                return new BaseResponse<ProductDto>
                {
                    Data = productDto,
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

        //public async Task<FileResult> DownloadProductTemplateAsync()
        //{

        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    var productUnits = await _productUnitService.GetProductUnitSelectList();
        //    var categories = await _categoryService.GetCategorySelectList();
        //    var brands = await _brandService.GetBrandSelectList();

        //    using var package = new ExcelPackage();
        //    var worksheet = package.Workbook.Worksheets.Add("Product Template");

        //    worksheet.Cells[1, 1].Value = "Name";
        //    worksheet.Cells[1, 2].Value = "Description";
        //    worksheet.Cells[1, 3].Value = "BarCode";
        //    worksheet.Cells[1, 4].Value = "Unit Price";
        //    worksheet.Cells[1, 5].Value = "Price Per Pack";
        //    worksheet.Cells[1, 6].Value = "Pack Price Markup";
        //    worksheet.Cells[1, 7].Value = "Total Item in Pack";
        //    worksheet.Cells[1, 8].Value = "Product Size";
        //    worksheet.Cells[1, 9].Value = "Category Name";
        //    worksheet.Cells[1, 10].Value = "Brand Name";
        //    worksheet.Cells[1, 11].Value = "Product Unit";
        //    worksheet.Cells[1, 12].Value = "Product Image URL";

        //    var categoryRangeName = "CategoryList";
        //    var brandRangeName = "BrandList";
        //    var productUnitRangeName = "ProductUnitList";

        //    PopulateDropdownList(worksheet, categories.Select(c => c.Text), categoryRangeName, 16);
        //    PopulateDropdownList(worksheet, brands.Select(b => b.Text), brandRangeName, 17);
        //    PopulateDropdownList(worksheet, productUnits.Select(u => u.Text), productUnitRangeName, 18);

        //    AddDropdownValidation(worksheet, categoryRangeName, 9);
        //    AddDropdownValidation(worksheet, brandRangeName, 10);
        //    AddDropdownValidation(worksheet, productUnitRangeName, 11);

        //    worksheet.Column(16).Hidden = true;
        //    worksheet.Column(17).Hidden = true;
        //    worksheet.Column(18).Hidden = true;

        //    var stream = new MemoryStream();
        //    package.SaveAs(stream);
        //    stream.Position = 0;

        //    return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //    {
        //        FileDownloadName = "ProductTemplate.xlsx"
        //    };
        //}

        public async Task<FileResult> DownloadProductTemplateAsync()
        {
            var productUnits = await _productUnitService.GetProductUnitSelectList();
            var categories = await _categoryService.GetCategorySelectList();
            var brands = await _brandService.GetBrandSelectList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Product Template");

            // Create headers
            CreateHeaders(worksheet);

            // Hidden dropdown columns
            const int categoryCol = 16;
            const int brandCol = 17;
            const int productUnitCol = 18;

            // Populate dropdown lists into hidden columns
            PopulateDropdownList(worksheet, categories.Select(c => c.Text), "CategoryList", categoryCol);
            PopulateDropdownList(worksheet, brands.Select(b => b.Text), "BrandList", brandCol);
            PopulateDropdownList(worksheet, productUnits.Select(u => u.Text), "ProductUnitList", productUnitCol);

            // Apply dropdowns to user-facing columns
            AddDropdownValidation(worksheet, "CategoryList", 9);
            AddDropdownValidation(worksheet, "BrandList", 10);
            AddDropdownValidation(worksheet, "ProductUnitList", 11);

            // Hide dropdown source columns
            worksheet.Column(categoryCol).Hide();
            worksheet.Column(brandCol).Hide();
            worksheet.Column(productUnitCol).Hide();

            // Format the worksheet
            FormatWorksheet(worksheet);

            // Create and return file result with proper disposal
            return CreateFileResult(workbook);
        }

        private void CreateHeaders(IXLWorksheet worksheet)
        {
            var headers = new[]
            {
        "Name", "Description", "BarCode", "Unit Price", "Price Per Pack",
        "Pack Price Markup", "Total Item in Pack", "Product Size",
        "Category Name", "Brand Name", "Product Unit", "Product Image URL"
    };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            }
        }

        private void FormatWorksheet(IXLWorksheet worksheet)
        {
            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Freeze header row
            worksheet.SheetView.FreezeRows(1);

            // Add borders to header row
            var headerRange = worksheet.Range(1, 1, 1, 12);
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private FileResult CreateFileResult(XLWorkbook workbook)
        {
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"ProductTemplate_{DateTime.Now:yyyy-MM-dd}.xlsx"
            };
        }

        private void AddDropdownValidation(IXLWorksheet worksheet, string rangeName, int columnIndex)
        {
            // Use a larger range to accommodate more rows (1000 instead of 100)
            var range = worksheet.Range(2, columnIndex, 1000, columnIndex);
            var validation = range.CreateDataValidation();
            validation.IgnoreBlanks = true;
            validation.InCellDropdown = true;
            validation.AllowedValues = XLAllowedValues.List;
            validation.List($"={rangeName}");  // Use List method
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "Invalid Selection";
            validation.ErrorMessage = "Please select a valid value from the dropdown list.";
            validation.ShowInputMessage = true;
            validation.InputTitle = "Selection Required";
            validation.InputMessage = "Please select from the available options.";
        }

        private void PopulateDropdownList(IXLWorksheet worksheet, IEnumerable<string> items, string rangeName, int columnIndex)
        {
            var itemList = items.ToList();

            // Guard against empty lists
            if (!itemList.Any())
            {
                worksheet.Cell(1, columnIndex).Value = "No options available";
                var singleRange = worksheet.Range(1, columnIndex, 1, columnIndex);
                singleRange.AddToNamed(rangeName);
                return;
            }

            int rowIndex = 1;
            foreach (var item in itemList)
            {
                worksheet.Cell(rowIndex++, columnIndex).Value = item ?? string.Empty;
            }

            var range = worksheet.Range(1, columnIndex, itemList.Count, columnIndex);
            range.AddToNamed(rangeName);
        }


        //private void PopulateDropdownList(ExcelWorksheet worksheet, IEnumerable<string> items, string rangeName, int columnIndex)
        //{
        //    var rowIndex = 2;
        //    foreach (var item in items)
        //    {
        //        worksheet.Cells[rowIndex++, columnIndex].Value = item;
        //    }
        //    worksheet.Names.Add(rangeName, worksheet.Cells[2, columnIndex, items.Count() + 1, columnIndex]);
        //}

        //private void AddDropdownValidation(ExcelWorksheet worksheet, string rangeName, int columnIndex)
        //{
        //    var validation = worksheet.DataValidations.AddListValidation(worksheet.Cells[2, columnIndex, 100, columnIndex].Address);
        //    validation.ShowErrorMessage = true;
        //    validation.ErrorTitle = "Invalid selection";
        //    validation.Error = "Please select a valid value from the list.";
        //    validation.Formula.ExcelFormula = $"={rangeName}";
        //}


        //public async Task UploadProductsFromExcelAsync(Stream excelStream)
        //{
        //    try
        //    {
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //        using var package = new ExcelPackage(excelStream);
        //        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        //        if (worksheet == null)
        //            throw new ArgumentException("The Excel file is empty.");

        //        var productList = new List<CreateProductRequest>();

        //        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        //        {
        //            var name = worksheet.Cells[row, 1]?.Value?.ToString()?.Trim();
        //            if (string.IsNullOrWhiteSpace(name))
        //                break;
        //            var description = worksheet.Cells[row, 2]?.Value?.ToString()?.Trim();
        //            var barCode = worksheet.Cells[row, 3]?.Value?.ToString()?.Trim();
        //            var unitPrice = decimal.TryParse(worksheet.Cells[row, 4]?.Value?.ToString(), out var unitPriceValue) ? unitPriceValue : 0;
        //            var pricePerPack = decimal.TryParse(worksheet.Cells[row, 5]?.Value?.ToString(), out var pricePerPackValue) ? pricePerPackValue : 0;
        //            var packPriceMarkup = decimal.TryParse(worksheet.Cells[row, 6]?.Value?.ToString(), out var packPriceMarkupValue) ? packPriceMarkupValue : 0;
        //            var totalItemInPack = int.TryParse(worksheet.Cells[row, 7]?.Value?.ToString(), out var totalItemValue) ? totalItemValue : 0;
        //            var productSize = double.TryParse(worksheet.Cells[row, 8]?.Value?.ToString(), out var productSizeValue) ? productSizeValue : 0;
        //            var categoryName = worksheet.Cells[row, 9]?.Value?.ToString()?.Trim();
        //            var brandName = worksheet.Cells[row, 10]?.Value?.ToString()?.Trim();
        //            var unitName = worksheet.Cells[row, 11]?.Value?.ToString()?.Trim();
        //            var productImage = worksheet.Cells[row, 12]?.Value?.ToString()?.Trim();

        //            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(unitName))
        //                continue;

        //            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
        //            var brand = await _brandRepository.GetBrandByNameAsync(brandName);
        //            var productUnit = await _productUnitRepository.GetProductUnitByNameAsync(unitName);

        //            if (category == null || productUnit == null)
        //                continue;

        //            var productRequest = new CreateProductRequest
        //            {
        //                Name = name,
        //                Description = description,
        //                BarCode = barCode,
        //                UnitPrice = unitPrice,
        //                PricePerPack = pricePerPack,
        //                PackPriceMarkup = packPriceMarkup,
        //                TotalItemInPack = totalItemInPack,
        //                ProductSize = productSize,
        //                CategoryId = category.Id,
        //                BrandId = brand?.Id,
        //                UnitId = productUnit.Id,
        //                ProductImageUrl = productImage
        //            };

        //            productList.Add(productRequest);
        //        }

        //        foreach (var productRequest in productList)
        //        {
        //            await CreateProductAsync(productRequest);
        //        }
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        throw new ApplicationException("There was an issue with the Excel file format. Please ensure it is correctly structured.", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("An error occurred while processing the Excel file. Please try again later.", ex);
        //    }
        //}

        public async Task UploadProductsFromExcelAsync(Stream excelStream)
        {
            try
            {
                using var workbook = new XLWorkbook(excelStream);
                var worksheet = workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    throw new ArgumentException("The Excel file is empty.");

                var productList = new List<CreateProductRequest>();

                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    var name = row.Cell(1).GetString().Trim();
                    if (string.IsNullOrWhiteSpace(name))
                        break;

                    var description = row.Cell(2).GetString().Trim();
                    var barCode = row.Cell(3).GetString().Trim();

                    var unitPrice = decimal.TryParse(row.Cell(4).GetString(), out var unitPriceValue) ? unitPriceValue : 0;
                    var pricePerPack = decimal.TryParse(row.Cell(5).GetString(), out var pricePerPackValue) ? pricePerPackValue : 0;
                    var packPriceMarkup = decimal.TryParse(row.Cell(6).GetString(), out var packPriceMarkupValue) ? packPriceMarkupValue : 0;
                    var totalItemInPack = int.TryParse(row.Cell(7).GetString(), out var totalItemValue) ? totalItemValue : 0;
                    var productSize = double.TryParse(row.Cell(8).GetString(), out var productSizeValue) ? productSizeValue : 0;

                    var categoryName = row.Cell(9).GetString().Trim();
                    var brandName = row.Cell(10).GetString().Trim();
                    var unitName = row.Cell(11).GetString().Trim();
                    var productImage = row.Cell(12).GetString().Trim();

                    if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(unitName))
                        continue;

                    var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
                    var brand = await _brandRepository.GetBrandByNameAsync(brandName);
                    var productUnit = await _productUnitRepository.GetProductUnitByNameAsync(unitName);

                    if (category == null || productUnit == null)
                        continue;

                    var productRequest = new CreateProductRequest
                    {
                        Name = name,
                        Description = description,
                        BarCode = barCode,
                        UnitPrice = unitPrice,
                        PricePerPack = pricePerPack,
                        PackPriceMarkup = packPriceMarkup,
                        TotalItemInPack = totalItemInPack,
                        ProductSize = productSize,
                        CategoryId = category.Id,
                        BrandId = brand?.Id,
                        UnitId = productUnit.Id,
                        ProductImageUrl = productImage
                    };

                    productList.Add(productRequest);
                }

                foreach (var productRequest in productList)
                {
                    await CreateProductAsync(productRequest);
                }
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException("There was an issue with the Excel file format. Please ensure it is correctly structured.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while processing the Excel file. Please try again later.", ex);
            }
        }
    }
}