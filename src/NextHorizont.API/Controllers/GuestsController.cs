using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Guests.Commands.CreateGuest;
using NextHorizont.Application.UseCases.Guests.Queries.SearchGuest;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class GuestsController : ApiControllerBase
{
    public record CreateGuestRequest(string FirstName, string LastName, string DocumentId, string? Email, string? Phone);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestRequest request)
    {
        var command = new CreateGuestCommand(TenantId, request.FirstName, request.LastName, request.DocumentId, request.Email, request.Phone);
        var result = await Mediator.Send(command);
        return Ok(new { GuestId = result });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("El parámetro keyword es requerido.");

        var query = new SearchGuestQuery(keyword, TenantId);
        var guests = await Mediator.Send(query);

        return Ok(guests.Select(g => new
        {
            g.Id,
            g.FirstName,
            g.LastName,
            DocumentId = g.IdentificationDocument,
            g.Email,
            g.Phone
        }));
    }
}
