using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Users.Commands.LoginUser;
using System.Threading.Tasks;

namespace NextHorizont.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    public record LoginRequest(string Username, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // En el login se asume que el TenantId viene en el header X-Tenant-Id
        var command = new LoginUserCommand(request.Username, request.Password, TenantId);
        
        var result = await Mediator.Send(command);

        return Ok(result);
    }
}
