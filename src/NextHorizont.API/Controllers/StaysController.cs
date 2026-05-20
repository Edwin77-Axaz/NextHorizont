using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Stays.Commands.CheckInStay;
using NextHorizont.Application.UseCases.Stays.Queries.GetActiveStays;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class StaysController : ApiControllerBase
{
    public record CheckInStayRequest(Guid LocationId, Guid GuestId, DateOnly CheckInDate, DateOnly CheckOutDate, decimal NightlyRate);

    [HttpPost("checkin")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInStayRequest request)
    {
        var command = new CheckInStayCommand(
            TenantId,
            request.LocationId,
            request.GuestId,
            request.CheckInDate,
            request.CheckOutDate,
            request.NightlyRate);

        var result = await Mediator.Send(command);
        return Ok(new { StayId = result });
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var query = new GetActiveStaysQuery(TenantId);
        var stays = await Mediator.Send(query);

        return Ok(stays.Select(s => new
        {
            s.Id,
            s.LocationId,
            s.GuestId,
            s.CheckInDate,
            s.CheckOutDate,
            s.NightlyRate,
            s.Status
        }));
    }
}
