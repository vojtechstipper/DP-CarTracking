using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Commands;

public class EditVehicleCommand : IRequest<string>
{
    public int LowBatteryThreshold { get; set; }
    public string Name { get; set; } = string.Empty;
    public string VehicleId { get; set; } = string.Empty;
    public double VirtualGarageRadius { get; set; }
    public int HistoryInDays { get; set; }
}

public class EditVehicleCommandHandler(IVehiclesRepository vehiclesRepository)
    : IRequestHandler<EditVehicleCommand, string>
{
    public async Task<string> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehiclesRepository.GetById(request.VehicleId);
        vehicle.ValidateIfNotNull(request.VehicleId);

        vehicle!.Name = request.Name;
        vehicle.Settings.LowBatteryThreshold = request.LowBatteryThreshold;
        vehicle.Settings.Radius = request.VirtualGarageRadius;
        vehicle.Settings.HistoryLenghtInDays = request.HistoryInDays;

        vehiclesRepository.Update(vehicle);
        await vehiclesRepository.SaveAsync();

        return vehicle.Id;
    }
}