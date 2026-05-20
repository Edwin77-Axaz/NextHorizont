using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.CashShifts.Commands.CloseCashShift;
using NextHorizont.Application.UseCases.CashShifts.Commands.OpenCashShift;
using NextHorizont.Application.UseCases.CashShifts.Queries.GetActiveCashShift;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class CashShiftsController : ApiControllerBase
{
    public record OpenCashShiftRequest(Guid UserId, decimal OpeningBalance);
    public record CloseCashShiftRequest(Guid UserId, decimal ClosingBalance, string? DenominationsClosing);

    [HttpPost("open")]
    public async Task<IActionResult> Open([FromBody] OpenCashShiftRequest request)
    {
        var command = new OpenCashShiftCommand(TenantId, request.UserId, request.OpeningBalance);
        var result = await Mediator.Send(command);
        return Ok(new { CashShiftId = result });
    }

    [HttpPost("close")]
    public async Task<IActionResult> Close([FromBody] CloseCashShiftRequest request)
    {
        var command = new CloseCashShiftCommand(TenantId, request.UserId, request.ClosingBalance, request.DenominationsClosing);
        var result = await Mediator.Send(command);
        return Ok(new { CashShiftId = result });
    }

    [HttpGet("active/{userId:guid}")]
    public async Task<IActionResult> GetActive(Guid userId)
    {
        var query = new GetActiveCashShiftQuery(TenantId, userId);
        var shift = await Mediator.Send(query);

        if (shift == null)
            return NotFound("No active cash shift found for this user.");

        return Ok(new
        {
            shift.Id,
            shift.UserId,
            shift.OpeningBalance,
            shift.OpeningTime,
            shift.Status
        });
    }
}
