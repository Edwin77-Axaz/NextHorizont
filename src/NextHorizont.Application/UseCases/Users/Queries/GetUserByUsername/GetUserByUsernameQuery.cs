using System;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Users.Queries.GetUserByUsername;

public record GetUserByUsernameQuery(string Username, Guid TenantId) : IRequest<User?>;
