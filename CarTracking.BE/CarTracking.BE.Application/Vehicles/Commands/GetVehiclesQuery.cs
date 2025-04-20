using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CarTracking.BE.Application.Vehicles.Commands;

public record GetVehiclesQuery(string AccountId) : IRequest<VehiclesResponse>;

public class GetVehiclesQueryHandler(CarTrackingDbContext context, IMemoryCache cache)
    : IRequestHandler<GetVehiclesQuery, VehiclesResponse>
{
    public async Task<VehiclesResponse> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await context.Vehicles
            .Where(x => x.AccountId == request.AccountId)
            .Select(x => new VehicleDto()
                { Id = x.Id, Name = x.Name, IsAssigned = x.IsAssignedToDevice, DeviceId = x.DeviceId })
            .ToListAsync();


        foreach (var vehicle in vehicles.Where(x => x.DeviceId != null))
        {
            //First try retrieve data from cache
            var status = GetLastKnownStatusFromCache(vehicle.DeviceId!);
            if (status is not null)
            {
                vehicle.LastKnownLocation = CreateVehicleLastKnownLocation(status);
                vehicle.LastTimeSent = status.Received;
                vehicle.BatteryInfo = MapVehicleBatteryInfo(status);
                vehicle.LastTimeMoved = status.StartTime;
            }
            else
            {
                status = await context.Statuses.Include(x => x.Location)
                    .Where(x => x.Location != null)
                    .Where(x => x.VehicleId == vehicle.Id)
                    .OrderByDescending(x => x.Received)
                    .FirstOrDefaultAsync();

                if (status is not null)
                {
                    vehicle.LastKnownLocation = CreateVehicleLastKnownLocation(status);
                    vehicle.LastTimeSent = status.Received;
                    vehicle.BatteryInfo = MapVehicleBatteryInfo(status);
                    vehicle.LastTimeMoved = status.StartTime;
                }
            }
        }

        return new VehiclesResponse() { Vehicles = vehicles };
    }

    private static LocationDto CreateVehicleLastKnownLocation(Domain.Entities.Status status)
    {
        return new LocationDto()
        {
            Latitude = status.Location.Latitude,
            Longitude = status.Location.Longitude
        };
    }

    private static BatteryInfoDto MapVehicleBatteryInfo(Domain.Entities.Status status)
    {
        return new BatteryInfoDto()
        {
            ChargeLevel = status.BatteryInfo.ChargeLevel,
            IsCharging = status.BatteryInfo.IsCharging,
            IsEnergySaverOn = status.BatteryInfo.IsEnergySaverOn
        };
    }

    private Domain.Entities.Status? GetLastKnownStatusFromCache(string vehicleDeviceId)
    {
        if (cache.TryGetValue(vehicleDeviceId, out Domain.Entities.Status status))
        {
            return status;
        }

        return null;
    }
}