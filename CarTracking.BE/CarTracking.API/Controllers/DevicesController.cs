using CarTracking.BE.Application.Devices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
[Route("api/[controller]")]
public class DevicesController(IMediator mediator) : Controller
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RegisteredDeviceDto>> RegisterDevice([FromBody] RegisterDeviceCommand command)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        command.AccountId = accountId;
        return Ok(await mediator.Send(command));
    }

    [HttpPost("{id}/test-notification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RegisteredDeviceDto>> TestNotificationFromDevice([FromRoute] string id)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        return Ok(await mediator.Send(new TestNotificationFromDeviceCommand()
        {
            AccountId = accountId,
            DeviceId = id
        }));
    }
}