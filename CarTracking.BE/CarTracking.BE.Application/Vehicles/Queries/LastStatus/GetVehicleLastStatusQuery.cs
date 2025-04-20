using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Extensions;
using CarTracking.BE.Application.Repositories;
using MediatR;

namespace CarTracking.BE.Application.Vehicles.Queries.LastStatus;

public class GetVehicleLastStatusQuery : IRequest<VehicleStatusDto>
{
    public required string VehicleId { get; set; }
}

public class GetVehicleLastStatusQueryHandler(IStatusRepository statusRepository)
    : IRequestHandler<GetVehicleLastStatusQuery, VehicleStatusDto>
{
    public async Task<VehicleStatusDto> Handle(GetVehicleLastStatusQuery request, CancellationToken cancellationToken)
    {
        var lastStatus = await statusRepository.GetLastStatusForVehicle(request.VehicleId);
        lastStatus.ValidateIfNotNull(request.VehicleId);

        return new VehicleStatusDto()
        {
            Location = new LocationDto()
            {
                Latitude = lastStatus!.Location.Latitude,
                Longitude = lastStatus.Location.Longitude
            },
            BatteryInfo = new BatteryInfoDto()
            {
                ChargeLevel = lastStatus.BatteryInfo.ChargeLevel,
                IsCharging = lastStatus.BatteryInfo.IsCharging,
                IsEnergySaverOn = lastStatus.BatteryInfo.IsEnergySaverOn
            },
            Sent = lastStatus.Received,
            VehicleName = lastStatus.Vehicle.Name,
            IsVirtualGarageActive = lastStatus.Vehicle.VirtualGarageIsEnabled
        };
    }
}