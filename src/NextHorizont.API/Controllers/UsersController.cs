using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHorizont.Application.UseCases.Users.Commands.CreateUser;
using NextHorizont.Application.UseCases.Users.Queries.GetUserByUsername;

namespace NextHorizont.Api.Controllers;

[Authorize]
public class UsersController : ApiControllerBase
{
    public record CreateUserRequest(string Username, string Password, Guid RoleId);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(TenantId, request.RoleId, request.Username, request.Password);
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetByUsername), new { username = request.Username }, new { Id = result });
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var query = new GetUserByUsernameQuery(username, TenantId);
        var user = await Mediator.Send(query);
        
        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.TenantId,
            user.Username,
            user.Role,
            user.IsActive,
            user.CreatedAt
        });
    }
}
