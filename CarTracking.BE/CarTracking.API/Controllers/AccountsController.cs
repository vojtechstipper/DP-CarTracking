using CarTracking.BE.Application.Accounts.Commands;
using CarTracking.BE.Application.Accounts.Commands.JoinAccount;
using CarTracking.BE.Application.Accounts.Commands.OpenAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
[Route("api/[controller]")]
public class AccountsController(IMediator mediator) : Controller
{
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateOrJoinAccountDto>> CreateAccount()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

        return Ok(await mediator.Send(new CreateAccountCommand { UserId = userId }));
    }

    
    [HttpPost("join")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateOrJoinAccountDto>> CreateAccount([FromBody] JoinAccountCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [HttpPost("open")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OpenAccountDto>> OpenAccountForConnection()
    {
        return Ok(await mediator.Send(new OpenAccountCommand()));
    }
}