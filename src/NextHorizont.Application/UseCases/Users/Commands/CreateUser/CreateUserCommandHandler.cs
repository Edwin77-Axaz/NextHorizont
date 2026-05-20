using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Application.Interfaces.Security;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista un usuario con el mismo username en el tenant
        var existing = await _userRepository.GetByUsernameAsync(request.Username, request.TenantId);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe un usuario con el nombre '{request.Username}' en este tenant.");

        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = new User(
            Guid.NewGuid(),
            request.TenantId,
            request.RoleId,
            request.Username,
            hashedPassword);

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
