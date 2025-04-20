using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Vehicles;
using CarTracking.BE.Application.Vehicles.Commands;
using CarTracking.BE.Application.Vehicles.Commands.Locations;
using CarTracking.BE.Application.Vehicles.Commands.Trips;
using CarTracking.BE.Application.Vehicles.Queries;
using CarTracking.BE.Application.Vehicles.Queries.LastStatus;
using CarTracking.BE.Application.Vehicles.Queries.Locations;
using CarTracking.BE.Application.Vehicles.Queries.Trips;
using CarTracking.BE.Application.Vehicles.Queries.VehicleById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController(IMediator mediator) : Controller
{
    [HttpGet("basic-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehiclesResponse>> GetVehicles()
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;

        return Ok(await mediator.Send(new GetVehiclesQuery(accountId!)));
    }

    [HttpGet("{vehicleId}/last-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehiclesList>> GetVehicleLastStatus([FromRoute] GetVehicleLastStatusQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpGet("{vehicleId}/trips")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleTripsListDto>> GetVehicleTrips([FromRoute] GetVehicleTripQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpPost("{vehicleId}/trips/generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GenerateVehicleTrips(
        [FromRoute] GenerateTripsFromStatusesCommand query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpGet("{vehicleId}/trips/v2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleTripsListDto>> GetVehicleTripsV2([FromRoute] GetVehicleTripQueryV2 query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpDelete("{vehicleId}/trips/{tripId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> DeleteTripForVehicle([FromRoute] DeleteVehicleTripByIdCommand query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehiclesList>> GetVehiclesForAccount()
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        return Ok(await mediator.Send(new GetVehiclesForAccountQuery(accountId)));
    }

    [HttpGet("{vehicleId}/config")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleConfigDto>> GetVehicleById([FromRoute] string vehicleId)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        return Ok(await mediator.Send(new GetVehicleByIdQuery(vehicleId)));
    }

    [HttpPut("{vehicleId}/config")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleConfigDto>> EditVehicle([FromRoute] string vehicleId,
        [FromBody] EditVehicleCommand command)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        command.VehicleId = vehicleId;
        return Ok(await mediator.Send(command));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> AddNewVehicle([FromBody] AddNewVehicleCommand command)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        command.AccountId = accountId;
        return Ok(await mediator.Send(command));
    }

    [HttpPost("{vehicleId}/virtual-garage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> EnableVirtualGarage([FromRoute] string vehicleId,
        [FromBody] EnableVirtualGarageCommand command
    )
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        command.AccountId = accountId;
        command.VehicleId = vehicleId;
        return Ok(await mediator.Send(command));
    }

    [HttpPut("{vehicleId}/virtual-garage/disable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> EnableVirtualGarage([FromRoute] string vehicleId)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        var command = new DisableVirtualGarageCommand();
        command.AccountId = accountId;
        command.VehicleId = vehicleId;
        return Ok(await mediator.Send(command));
    }


    [HttpPost("{vehicleId}/locations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> AddLocationToVehicle([FromRoute] string vehicleId,
        [FromBody] AddLocationToVehicleCommand command
    )
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        command.AccountId = accountId;
        command.VehicleId = vehicleId;
        return Ok(await mediator.Send(command));
    }

    [HttpGet("{vehicleId}/locations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleLocationsDto>> GetLocationsForVehicle([FromRoute] string vehicleId
    )
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        return Ok(await mediator.Send(new GetLocationsForVehicleQuery(vehicleId)));
    }

    [HttpDelete("{vehicleId}/locations/{locationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> GetLocationsForVehicle([FromRoute] string vehicleId,
        [FromRoute] string locationId
    )
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        return Ok(await mediator.Send(new DeleteVehicleLocationCommand(vehicleId, locationId)));
    }

    [HttpPut("{vehicleId}/locations/{locationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> UpdateLocation([FromRoute] string vehicleId, [FromRoute] string locationId
        , [FromBody] UpdateVehicleLocationCommand command)
    {
        var accountId = User.Claims.FirstOrDefault(x => x.Type == "AccountId")!.Value;
        command.AccountId = accountId;
        command.VehicleId = vehicleId;
        command.LocationId = locationId;
        return Ok(await mediator.Send(command));
    }
}