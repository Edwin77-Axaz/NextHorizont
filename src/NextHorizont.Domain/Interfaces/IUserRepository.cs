using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, Guid tenantId);
    Task<User?> GetByUsernameAsync(string username, Guid tenantId);
    Task<User?> GetByUsernameGlobalAsync(string username);
    Task<IEnumerable<User>> GetByRoleAsync(Guid roleId, Guid tenantId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}
