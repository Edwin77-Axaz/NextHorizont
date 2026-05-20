using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
