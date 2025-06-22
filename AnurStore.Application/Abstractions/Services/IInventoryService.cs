using AnurStore.Application.DTOs;
using AnurStore.Application.Wrapper;
using AnurStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnurStore.Application.Abstractions.Services
{
    public interface IInventoryService
    {
        Task<BaseResponse<IEnumerable<InventoryDto>>> GetAllInventories();
        Task<BaseResponse<IEnumerable<InventoryDto>>> GetInventoriesByStatusAsync(StockStatus status);

    }
}
