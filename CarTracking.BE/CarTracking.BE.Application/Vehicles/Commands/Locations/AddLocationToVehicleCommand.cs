using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Domain.Entities;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands.Locations;

public class AddLocationToVehicleCommand : IRequest<string>
{
    public string VehicleId { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public LocationDto Location { get; set; }
    public int Radius { get; set; }
    public string LocationName { get; set; }
    public bool IsUsedForVirtualGarage { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}

public class AddLocationToVehicleCommandHandler(IVehicleLocationsRepository vehicleLocationsRepository)
    : IRequestHandler<AddLocationToVehicleCommand, string>
{
    public async Task<string> Handle(AddLocationToVehicleCommand request, CancellationToken cancellationToken)
    {
        var location = new VehicleLocation
        {
            Latitude = request.Location.Latitude,
            Longitude = request.Location.Longitude,
            Radius = request.Radius,
            Name = request.LocationName,
            VehicleId = request.VehicleId,
            IsUsedForVirtualGarage = request.IsUsedForVirtualGarage,
            FromTime = request.FromTime,
            ToTime = request.ToTime
        };

        vehicleLocationsRepository.Add(location);
        await vehicleLocationsRepository.SaveAsync();
        return location.Id;
    }
}