using System;
using MediatR;

namespace NextHorizont.Application.UseCases.Users.Commands.CreateUser;

public record CreateUserCommand(
    Guid TenantId,
    Guid RoleId,
    string Username,
    string Password
) : IRequest<Guid>;
