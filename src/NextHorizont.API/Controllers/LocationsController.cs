using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Locations.Commands.ChangeLocationStatus;
using NextHorizont.Application.UseCases.Locations.Queries.GetLocationsByArea;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class LocationsController : ApiControllerBase
{
    public record ChangeStatusRequest(RoomStatus NewStatus);

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest request)
    {
        var command = new ChangeLocationStatusCommand(TenantId, id, request.NewStatus);
        var result = await Mediator.Send(command);
        return Ok(new { LocationId = result });
    }

    [HttpGet("area/{areaId:guid}")]
    public async Task<IActionResult> GetByArea(Guid areaId)
    {
        var query = new GetLocationsByAreaQuery(areaId, TenantId);
        var locations = await Mediator.Send(query);

        return Ok(locations.Select(l => new
        {
            l.Id,
            l.Name,
            l.Type,
            l.RoomStatus
        }));
    }
}
