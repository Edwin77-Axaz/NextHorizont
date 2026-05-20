using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Infraestructura.Persistence;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class StayRepository : IStayRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public StayRepository(AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory)
    {
        _dbContext = dbContext;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Stay?> GetByIdAsync(Guid id, Guid tenantId)
    {
        return await _dbContext.Stays
            .FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);
    }

    public async Task<IEnumerable<Stay>> GetActiveStaysAsync(Guid tenantId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"
            SELECT * FROM ""Stays"" 
            WHERE ""TenantId"" = @TenantId 
            AND ""Status"" IN ('CheckIn', 'Reservada')";
            
        return await connection.QueryAsync<Stay>(sql, new { TenantId = tenantId });
    }

    public async Task AddAsync(Stay stay)
    {
        await _dbContext.Stays.AddAsync(stay);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Stay stay)
    {
        _dbContext.Stays.Update(stay);
        await _dbContext.SaveChangesAsync();
    }
}
