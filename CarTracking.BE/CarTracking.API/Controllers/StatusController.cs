using CarTracking.BE.Application.Status.Commands;
using CarTracking.BE.Application.Statuses.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class StatusController(IMediator mediator) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostStatus([FromBody] SendStatusCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPost("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckStatus()
    {
        var command = new CheckStatusCommand();
        return Ok(await mediator.Send(command));
    }
}