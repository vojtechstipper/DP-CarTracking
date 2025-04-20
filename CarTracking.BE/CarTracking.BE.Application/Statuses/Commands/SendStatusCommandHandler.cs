using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Helpers;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using CarTracking.BE.Application.Status.Commands;
using CarTracking.BE.Domain.Entities;
using MediatR;

namespace CarTracking.BE.Application.Statuses.Commands;

public class SendStatusCommandHandler(
    StatusService statusService,
    IVehiclesRepository vehiclesRepository,
    IVehicleLocationsRepository vehicleLocationsRepository)
    : IRequestHandler<SendStatusCommand, Unit>
{
    public async Task<Unit> Handle(SendStatusCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetVehicleByDeviceId(request.DeviceId);
        vehicle.ValidateIfNotNull(request.DeviceId);

        var namedLocations = await vehicleLocationsRepository.GetVehicleLocationsByVehicleId(vehicle!.Id);
        var currentNamedLocation = TryGetCurrentNamedLocation(request.Location, namedLocations);
        
        var status = new Domain.Entities.Status
        {
            DeviceId = vehicle!.Device!.Id,
            Device = vehicle.Device,
            Location = new Location()
            {
                Accuracy = request.Location.Accuracy,
                Altitude = request.Location.Altitude,
                Latitude = request.Location.Latitude,
                Longitude = request.Location.Longitude,
                Speed = request.Location.Speed
            },
            Received = DateTime.UtcNow,
            StartTime = DateTime.UtcNow,
            Sent = request.Sent,
            Vehicle = vehicle,
            BatteryInfo = new BatteryInfo()
            {
                ChargeLevel = request.BatteryInfo.ChargeLevel,
                IsCharging = request.BatteryInfo.IsCharging,
                IsEnergySaverOn = request.BatteryInfo.IsEnergySaverOn
            },
            NamedVehicleLocationId = currentNamedLocation?.Id
        };

        await statusService.AddStatus(status, vehicle.AccountId);

        return Unit.Value;
    }

    private VehicleLocation? TryGetCurrentNamedLocation(SendStatusCommand.LocationDto requestLocation,
        List<VehicleLocation> namedLocations)
    {
        return VehicleLocationHelper.FindNearestLocation(new LocationDto()
        {
            Latitude = requestLocation.Latitude,
            Longitude = requestLocation.Longitude
        }, namedLocations);
    }
}