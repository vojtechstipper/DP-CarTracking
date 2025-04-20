using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands.Locations;

public record DeleteVehicleLocationCommand(string VehicleId, string LocationId) : IRequest<string>;

public class DeleteVehicleLocationCommandHandler(
    IVehicleLocationsRepository vehicleLocationsRepository,
    IVehiclesRepository vehiclesRepository) : IRequestHandler<DeleteVehicleLocationCommand, string>
{
    public async Task<string> Handle(DeleteVehicleLocationCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);

        var location = await vehicleLocationsRepository.GetById(request.LocationId);
        location.ValidateIfNotNull(request.LocationId);

        vehicleLocationsRepository.Delete(location!);

        await vehicleLocationsRepository.SaveAsync();

        return vehicle.Id;
    }
}