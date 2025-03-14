﻿using AnurStore.Domain.Entities;
using System.Threading.Tasks;

namespace AnurStore.Application.Abstractions.Repositories
{
    public interface IProductPurchaseRepository
    {
        Task<ProductPurchase> CreateAsync(ProductPurchase productPurchase);
        Task<IList<ProductPurchase>> GetAllAsync();
        Task<ProductPurchase> GetByIdAsync(string id); 
        Task<bool> UpdateAsync(ProductPurchase productPurchase); 
    }
}