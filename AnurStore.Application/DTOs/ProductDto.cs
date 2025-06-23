using AnurStore.Application.Abstractions.Repositories;
using AnurStore.Application.DTOs;
using AnurStore.Application.RequestModel;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Entities;
using AnurStore.Domain.Enums;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Helpers;
using SkiaSharp;
using static QuestPDF.Helpers.Colors;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.InteropServices;
using System;
using ZXing.QrCode.Internal;

namespace AnurStore.Application.DTOs;

public  class ProductDto 
{
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
    public double Size { get; set; }
    public string? UnitName { get; set; }
    public string Id { get; set; } = NewId.Next().ToSequentialGuid().ToString();
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}


