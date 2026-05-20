using System;
using System.Collections.Generic;
using MediatR;
using NextHorizont.Domain.Entities;

namespace NextHorizont.Application.UseCases.Orders.Queries.GetActiveOrders;

public record GetActiveOrdersQuery(Guid TenantId) : IRequest<IEnumerable<Order>>;
