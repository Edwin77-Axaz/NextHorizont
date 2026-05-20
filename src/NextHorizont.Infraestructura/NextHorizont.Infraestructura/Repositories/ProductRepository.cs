using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Infraestructura.Caching;
using NextHorizont.Infraestructura.Persistence;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ICacheService _cacheService;

    public ProductRepository(AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _sqlConnectionFactory = sqlConnectionFactory;
        _cacheService = cacheService;
    }

    public async Task<Product?> GetByIdAsync(Guid id, Guid tenantId)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, Guid tenantId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM ""Products"" WHERE ""TenantId"" = @TenantId AND ""CategoryId"" = @CategoryId AND ""IsActive"" = true";
        return await connection.QueryAsync<Product>(sql, new { TenantId = tenantId, CategoryId = categoryId });
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(Guid tenantId)
    {
        var cacheKey = $"tenant:{tenantId}:products:active";
        var cachedProducts = await _cacheService.GetAsync<IEnumerable<Product>>(cacheKey);
        
        if (cachedProducts != null)
        {
            return cachedProducts;
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM ""Products"" WHERE ""TenantId"" = @TenantId AND ""IsActive"" = true";
        var products = await connection.QueryAsync<Product>(sql, new { TenantId = tenantId });
        
        await _cacheService.SetAsync(cacheKey, products, TimeSpan.FromHours(4));
        return products;
    }

    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync($"tenant:{product.TenantId}:products:active");
    }

    public async Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync($"tenant:{product.TenantId}:products:active");
    }

    public async Task DeleteAsync(Guid id, Guid tenantId)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);
        if (product != null)
        {
            product.Deactivate();
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveAsync($"tenant:{tenantId}:products:active");
        }
    }
}
