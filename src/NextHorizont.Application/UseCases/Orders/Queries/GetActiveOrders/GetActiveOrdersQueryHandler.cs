using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Interfaces;

namespace NextHorizont.Application.UseCases.Orders.Queries.GetActiveOrders;

public class GetActiveOrdersQueryHandler : IRequestHandler<GetActiveOrdersQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetActiveOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepository.GetActiveOrdersAsync(request.TenantId);
    }
}
