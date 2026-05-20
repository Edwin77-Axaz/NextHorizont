using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;
using NextHorizont.Infraestructura.Contexts;

namespace NextHorizont.Infraestructura.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, Guid tenantId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId);
    }

    public async Task<User?> GetByUsernameAsync(string username, Guid tenantId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.TenantId == tenantId);
    }

    public async Task<User?> GetByUsernameGlobalAsync(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(Guid roleId, Guid tenantId)
    {
        return await _dbContext.Users.Where(u => u.RoleId == roleId && u.TenantId == tenantId).ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
