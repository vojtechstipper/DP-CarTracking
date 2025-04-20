using AutoMapper;
using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Queries.Locations;

public record GetLocationsForVehicleQuery(string VehicleId) : IRequest<VehicleLocationsDto>;

public class GetLocationsForVehicleQueryHandler(
    IVehicleLocationsRepository vehicleLocationsRepository,
    IVehiclesRepository vehiclesRepository,
    IMapper mapper)
    : IRequestHandler<GetLocationsForVehicleQuery, VehicleLocationsDto>
{
    public async Task<VehicleLocationsDto> Handle(GetLocationsForVehicleQuery request,
        CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);

        var locations = (await vehicleLocationsRepository.GetVehicleLocationsByVehicleId(request.VehicleId)).OrderBy(x=>x.Name).ToList();
        var locationsDto = mapper.Map<List<VehicleLocationDto>>(locations);

        return new VehicleLocationsDto
        {
            Locations = locationsDto,
            VehicleId = request.VehicleId
        };
    }
}