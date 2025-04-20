using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CarTracking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(CarTrackingDbContext context) : ControllerBase
{
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SeedDb()
    {
        Vehicle vehicle1 = new Vehicle()
        {
            Name = "Opel Astra",
            IsAssignedToDevice = true
        };

        Vehicle vehicle2 = new Vehicle()
        {
            Name = "Opel Corsa",
        };

        Account account = new Account()
        {
            Vehicles = new List<Vehicle> { vehicle1, vehicle2 }
        };
        Device device1 = new Device()
        {
            IsAdminDevice = false,
            Vehicle = vehicle1,
            Account = account,
            NotificationToken = "NoToken"
        };

        context.Devices.Add(device1);
        await context.SaveChangesAsync();
        return Ok();
    }
}