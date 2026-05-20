using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Orders.Commands.CreateOrder;
using NextHorizont.Application.UseCases.Orders.Queries.GetActiveOrders;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class OrdersController : ApiControllerBase
{
    public record CreateOrderRequest(Guid? LocationId, Guid? UserId, OrderOrigin Origin);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(TenantId, request.LocationId, request.UserId, request.Origin);
        var result = await Mediator.Send(command);
        return Ok(new { OrderId = result });
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var query = new GetActiveOrdersQuery(TenantId);
        var orders = await Mediator.Send(query);
        
        return Ok(orders.Select(o => new
        {
            o.Id,
            o.Status,
            o.Origin,
            o.Total,
            o.CreatedAt
        }));
    }
}
