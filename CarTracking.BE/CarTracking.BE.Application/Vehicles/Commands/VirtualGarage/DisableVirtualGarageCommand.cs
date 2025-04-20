using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands;

public class DisableVirtualGarageCommand : IRequest<string>
{
    public string VehicleId { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
}

public class DisableVirtualGarageCommandHandler(IVehiclesRepository vehiclesRepository)
    : IRequestHandler<DisableVirtualGarageCommand, string>
{
    public async Task<string> Handle(DisableVirtualGarageCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);


        vehicle!.VirtualGarageIsEnabled = false;
        vehiclesRepository.Update(vehicle);
        await vehiclesRepository.SaveAsync();

        return vehicle.Id;
    }
}