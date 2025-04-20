using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : Controller
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginDto>> PostStatus([FromBody] LoginUserCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginDto>> PostStatus([FromBody] RegisterUserCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPost("set-new-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string>> SetNewPassword([FromBody] SetNewPasswordCommand command)
    {
        return Ok(await mediator.Send(command));
    }
}