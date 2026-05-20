using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Domain.Enums;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class CashShiftRepository : ICashShiftRepository
{
    private readonly AppDbContext _dbContext;

    public CashShiftRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CashShift?> GetActiveShiftAsync(Guid tenantId, Guid userId)
    {
        return await _dbContext.CashShifts
            .FirstOrDefaultAsync(cs => cs.TenantId == tenantId && cs.UserId == userId && cs.Status == CashShiftStatus.Abierto);
    }

    public async Task AddAsync(CashShift shift)
    {
        await _dbContext.CashShifts.AddAsync(shift);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(CashShift shift)
    {
        _dbContext.CashShifts.Update(shift);
        await _dbContext.SaveChangesAsync();
    }
}
