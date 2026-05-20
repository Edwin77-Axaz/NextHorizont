using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Domain.Enums;
using NextHorizont.Infraestructura.Caching;
using NextHorizont.Infraestructura.Persistence;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ICacheService _cacheService;

    public OrderRepository(
        AppDbContext dbContext,
        ISqlConnectionFactory sqlConnectionFactory,
        ICacheService cacheService)
    {
        _dbContext = dbContext;
        _sqlConnectionFactory = sqlConnectionFactory;
        _cacheService = cacheService;
    }

    public async Task<Order?> GetByIdAsync(Guid id, Guid tenantId)
    {
        // Active orders change constantly. We might want to try EF Core if we need the tracking graph
        // For reads that don't need tracking, we can use Dapper + Redis.
        var cacheKey = $"tenant:{tenantId}:order:{id}";
        var cachedOrder = await _cacheService.GetAsync<Order>(cacheKey);
        if (cachedOrder != null)
        {
            return cachedOrder;
        }

        // For deep graphs (Order + Lines), EF Core with AsNoTracking is often easier, 
        // but Dapper is faster. Let's use EF Core for complex reads for now to avoid manual Dapper mapping
        var order = await _dbContext.Orders
            .Include(o => o.OrderLines)
            .Include(o => o.Payments)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id && o.TenantId == tenantId);

        if (order != null)
        {
            await _cacheService.SetAsync(cacheKey, order, TimeSpan.FromMinutes(30));
        }

        return order;
    }

    public async Task<IEnumerable<Order>> GetActiveOrdersAsync(Guid tenantId)
    {
        // Active orders list (Hot State) - perfect for Dapper
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"
            SELECT * FROM ""Orders"" 
            WHERE ""TenantId"" = @TenantId 
            AND ""Status"" IN ('Abierta', 'EnviadaACocina')";
            
        return await connection.QueryAsync<Order>(sql, new { TenantId = tenantId });
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        await _cacheService.SetAsync($"tenant:{order.TenantId}:order:{order.Id}", order, TimeSpan.FromMinutes(30));
    }

    public async Task UpdateAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
        
        // Remove from cache to force refresh, or update the cache
        var cacheKey = $"tenant:{order.TenantId}:order:{order.Id}";
        if (order.Status == OrderStatus.Cancelada || order.Status == OrderStatus.Pagada)
        {
            await _cacheService.RemoveAsync(cacheKey); // Finished orders don't need hot cache
        }
        else
        {
            await _cacheService.SetAsync(cacheKey, order, TimeSpan.FromMinutes(30));
        }
    }
}
