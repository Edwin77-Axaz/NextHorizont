using System;
using System.Threading.Tasks;
using Dapper;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Infraestructura.Caching;
using NextHorizont.Infraestructura.Persistence;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ICacheService _cacheService;

    public TenantRepository(
        AppDbContext dbContext,
        ISqlConnectionFactory sqlConnectionFactory,
        ICacheService cacheService)
    {
        _dbContext = dbContext;
        _sqlConnectionFactory = sqlConnectionFactory;
        _cacheService = cacheService;
    }

    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        // 1. Try Cache
        var cacheKey = $"tenant:{id}";
        var cachedTenant = await _cacheService.GetAsync<Tenant>(cacheKey);
        if (cachedTenant != null)
        {
            return cachedTenant;
        }

        // 2. Try Dapper (Fast Read)
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM ""Tenants"" WHERE ""Id"" = @Id AND ""IsActive"" = true";
        var tenant = await connection.QueryFirstOrDefaultAsync<Tenant>(sql, new { Id = id });

        // 3. Set Cache
        if (tenant != null)
        {
            await _cacheService.SetAsync(cacheKey, tenant, TimeSpan.FromHours(24));
        }

        return tenant;
    }

    public async Task AddAsync(Tenant tenant)
    {
        await _dbContext.Tenants.AddAsync(tenant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tenant tenant)
    {
        _dbContext.Tenants.Update(tenant);
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync($"tenant:{tenant.Id}");
    }

    public async Task DeleteAsync(Guid id)
    {
        var tenant = await _dbContext.Tenants.FindAsync(id);
        if (tenant != null)
        {
            tenant.Deactivate();
            _dbContext.Tenants.Update(tenant);
            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveAsync($"tenant:{id}");
        }
    }
}
