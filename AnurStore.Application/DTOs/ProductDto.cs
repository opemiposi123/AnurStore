using MassTransit;

namespace AnurStore.Application.DTOs;

public  class ProductDto 
{
    public string Id { get; set; } = NewId.Next().ToSequentialGuid().ToString();
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string? BarCode { get; set; }

    public decimal? PricePerPack { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal PackPriceMarkup { get; set; }

    public decimal UnitPriceMarkup { get; set; }

    public string? ProductImageUrl { get; set; } 

    public int TotalItemInPack { get; set; }

    public string SizeWithUnit { get; set; }

    public string CategoryId { get; set; } = default!;
    public string? CategoryName { get; set; }    
    public string? BrandName { get; set; }
    public string? BrandId { get; set; } 
    public string? UnitId  { get; set; } 
    public double Size { get; set; }
    public string? UnitName { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}


