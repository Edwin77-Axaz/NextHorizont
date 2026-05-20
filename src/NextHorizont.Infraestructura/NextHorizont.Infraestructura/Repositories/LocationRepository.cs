using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Domain.Enums;
using NextHorizont.Infraestructura.Persistence;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public LocationRepository(AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory)
    {
        _dbContext = dbContext;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Location?> GetByIdAsync(Guid id, Guid tenantId)
    {
        return await _dbContext.Locations.FirstOrDefaultAsync(l => l.Id == id && l.TenantId == tenantId);
    }

    public async Task<IEnumerable<Location>> GetByAreaAsync(Guid areaId, Guid tenantId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM ""Locations"" WHERE ""TenantId"" = @TenantId AND ""AreaId"" = @AreaId AND ""IsActive"" = true";
        return await connection.QueryAsync<Location>(sql, new { TenantId = tenantId, AreaId = areaId });
    }

    public async Task<IEnumerable<Location>> GetByRoomStatusAsync(RoomStatus status, Guid tenantId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM ""Locations"" WHERE ""TenantId"" = @TenantId AND ""RoomStatus"" = @Status AND ""IsActive"" = true";
        return await connection.QueryAsync<Location>(sql, new { TenantId = tenantId, Status = status.ToString() });
    }

    public async Task<IEnumerable<Location>> GetByTypeAsync(LocationType type, Guid tenantId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"SELECT * FROM ""Locations"" WHERE ""TenantId"" = @TenantId AND ""Type"" = @Type AND ""IsActive"" = true";
        return await connection.QueryAsync<Location>(sql, new { TenantId = tenantId, Type = type.ToString() });
    }

    public async Task AddAsync(Location location)
    {
        await _dbContext.Locations.AddAsync(location);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Location location)
    {
        _dbContext.Locations.Update(location);
        await _dbContext.SaveChangesAsync();
    }
}
