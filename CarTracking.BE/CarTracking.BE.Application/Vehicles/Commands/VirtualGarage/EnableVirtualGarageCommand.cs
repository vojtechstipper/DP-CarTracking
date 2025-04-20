using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Domain.Entities;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands;

public class EnableVirtualGarageCommand : IRequest<string>
{
    public required DateTime EndOfValidity { get; set; }
    public string VehicleId { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
}

public class EnableVirtualGarageCommandHandler(
    IVehiclesRepository vehiclesRepository,
    IStatusRepository statusRepository) : IRequestHandler<EnableVirtualGarageCommand, string>
{
    public async Task<string> Handle(EnableVirtualGarageCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);

        var lastStatus = await statusRepository.GetLastStatusForVehicle(vehicle!.Id);
        vehicle.ValidateIfNotNull($"last status for vehicle with id: {vehicle.Id}");

        vehicle.VirtualGarageIsEnabled = true;
        vehicle.VirtualGarage = new VirtualGarage()
        {
            ValidTill = request.EndOfValidity,
            Latitude = lastStatus!.Location.Latitude,
            Longitude = lastStatus!.Location.Longitude
        };

        vehiclesRepository.Update(vehicle);
        await vehiclesRepository.SaveAsync();
        return vehicle.Id;
    }
}