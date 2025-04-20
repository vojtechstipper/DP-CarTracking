using AutoMapper;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Queries.VehicleById;

public record GetVehicleByIdQuery(string VehicleId) : IRequest<VehicleConfigDto>;

public class GetVehicleByIdQueryHandler(IVehiclesRepository vehiclesRepository, IMapper mapper)
    : IRequestHandler<GetVehicleByIdQuery, VehicleConfigDto>
{
    public async Task<VehicleConfigDto> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);

        return mapper.Map<VehicleConfigDto>(vehicle);
    }
}