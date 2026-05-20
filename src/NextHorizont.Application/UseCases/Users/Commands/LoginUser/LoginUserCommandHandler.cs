using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Application.Interfaces.Security;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, request.TenantId);

        if (user is null || user.IsActive != true)
            throw new UnauthorizedAccessException("Credenciales inválidas o usuario inactivo.");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginUserResult(token, user.Id, user.Username, user.TenantId);
    }
}
