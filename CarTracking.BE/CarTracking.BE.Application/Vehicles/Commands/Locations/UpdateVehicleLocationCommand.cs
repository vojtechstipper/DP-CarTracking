using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands.Locations;

public class UpdateVehicleLocationCommand : IRequest<string>
{
    public LocationDto Location { get; set; }
    public string LocationName { get; set; }
    public int Radius { get; set; }
    public string AccountId { get; set; } = string.Empty;
    public string LocationId { get; set; } = string.Empty;
    public string VehicleId { get; set; } = string.Empty;
    public bool IsUsedForVirtualGarage { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}

public class UpdateVehicleLocationCommandHandler(
    IVehicleLocationsRepository vehicleLocationsRepository,
    IVehiclesRepository vehiclesRepository) : IRequestHandler<UpdateVehicleLocationCommand, string>
{
    public async Task<string> Handle(UpdateVehicleLocationCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);

        var location = await vehicleLocationsRepository.GetById(request.LocationId);
        location.ValidateIfNotNull(request.LocationId);

        location!.Name = request.LocationName;
        location.Radius = request.Radius;
        location.Latitude = request.Location.Latitude;
        location.Longitude = request.Location.Longitude;
        location.IsUsedForVirtualGarage = request.IsUsedForVirtualGarage;
        location.FromTime = request.FromTime;
        location.ToTime = request.ToTime;

        vehicleLocationsRepository.Update(location);
        await vehicleLocationsRepository.SaveAsync();
        return location.Id;
    }
}