using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, Guid tenantId);
    Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, Guid tenantId);
    Task<IEnumerable<Product>> GetActiveProductsAsync(Guid tenantId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id, Guid tenantId);
}
