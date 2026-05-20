using System;
using MediatR;

namespace NextHorizont.Application.UseCases.Users.Commands.LoginUser;

public record LoginUserCommand(
    string Username,
    string Password
) : IRequest<LoginUserResult>;

public record LoginUserResult(
    string Token,
    Guid UserId,
    string Username,
    Guid TenantId
);
