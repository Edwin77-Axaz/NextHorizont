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

public class GuestRepository : IGuestRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GuestRepository(AppDbContext dbContext, ISqlConnectionFactory sqlConnectionFactory)
    {
        _dbContext = dbContext;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Guest?> GetByIdAsync(Guid id, Guid tenantId)
    {
        return await _dbContext.Guests.FirstOrDefaultAsync(g => g.Id == id && g.TenantId == tenantId);
    }

    public async Task<Guest?> GetByDocumentAsync(string identificationDocument, Guid tenantId)
    {
        return await _dbContext.Guests.FirstOrDefaultAsync(g => g.IdentificationDocument == identificationDocument && g.TenantId == tenantId);
    }

    public async Task<IEnumerable<Guest>> SearchAsync(string searchTerm, Guid tenantId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = @"
            SELECT * FROM ""Guests"" 
            WHERE ""TenantId"" = @TenantId 
            AND (""FirstName"" ILIKE @SearchTerm OR ""LastName"" ILIKE @SearchTerm OR ""IdentificationDocument"" ILIKE @SearchTerm)";
            
        return await connection.QueryAsync<Guest>(sql, new { TenantId = tenantId, SearchTerm = $"%{searchTerm}%" });
    }

    public async Task AddAsync(Guest guest)
    {
        await _dbContext.Guests.AddAsync(guest);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guest guest)
    {
        _dbContext.Guests.Update(guest);
        await _dbContext.SaveChangesAsync();
    }
}
