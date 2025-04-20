using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Application.Helpers;
using CarTracking.BE.Application.Options;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CarTracking.BE.Application.Services;

public class StatusService(
    CarTrackingDbContext context,
    IVehiclesRepository vehiclesRepository,
    IMemoryCache memoryCache,
    INotificationSender notificationSender,
    IOptions<CacheSettings> cacheOptions,
    ICacheService cacheService,
    IDeviceRepository deviceRepository,
    IVehicleLocationsRepository vehicleLocationsRepository)
{
    public async Task AddStatus(Domain.Entities.Status status, string accountId)
    {
        (var vehicleStatus, string? locationNamedId) = await CreateNewOrUpdateExistingStatus(status);

        var adminDevices = (await deviceRepository.GetAdminDevicesForAccount(accountId))
            .Select(x => x.NotificationToken).ToList();

        var locations =
            await vehicleLocationsRepository.GetVehicleLocationsForVirtualGarageByVehicleId(status.Vehicle.Id);

        if (adminDevices.Any())
        {
            CreateCacheEntry(vehicleStatus, adminDevices);

            await SendLowBatteryNotification(status, vehicleStatus, adminDevices);
            await CheckVirtualGarage(status, adminDevices);
            await CheckLocationLeft(status, vehicleStatus, adminDevices, locations, locationNamedId);
        }
    }

    private async Task CheckLocationLeft(Domain.Entities.Status status, Domain.Entities.Status vehicleStatus,
        List<string> adminDevices, List<VehicleLocation> locations, string? lastNamedLocationId)
    {
        if (lastNamedLocationId is not null && string.IsNullOrEmpty(vehicleStatus.NamedVehicleLocationId))
        {
            var location = locations.FirstOrDefault(x => x.Id == lastNamedLocationId);
            if (location is not null && location.IsUsedForVirtualGarage && TimeIsInRange(location))
            {
                await notificationSender.SendNotificationsAsync(new NotificationDto()
                {
                    Title = $"{status.Vehicle.Name} has left {location.Name}",
                    Body =
                        $"Location left \nLeft at: {DateTimeConverterHelper.ConvertToCentralEuropeTimeZone(status.Received):HH:mm:ss}",
                    Data = new()
                }, adminDevices);
            }
        }
    }

    private bool TimeIsInRange(VehicleLocation location)
    {
        var timeFrom = location.FromTime;
        var timeTo = location.ToTime;
        var now = DateTime.Now.TimeOfDay;

        return timeFrom <= timeTo
            ? now >= timeFrom && now <= timeTo // Normal case (e.g., 08:00 - 18:00)
            : now >= timeFrom || now <= timeTo; // Overnight case (e.g., 22:00 - 06:00)
    }

    private async Task CheckVirtualGarage(Domain.Entities.Status status, List<string> adminDevices)
    {
        if (!status.Vehicle.VirtualGarageIsEnabled || status.Vehicle.VirtualGarage.ValidTill < DateTime.Now)
        {
            return;
        }

        var distance = GetDistanceInMeters(status.Location.Latitude, status.Location.Longitude,
            status.Vehicle.VirtualGarage.Latitude, status.Vehicle.VirtualGarage.Longitude);

        string cacheKey = $"VirtualGarageNotification:{status.Vehicle.Id}";
        var wasAlreadyNotified = cacheService.GetVirtualGarageNotified(cacheKey);

        if (distance > status.Vehicle.Settings.Radius && !wasAlreadyNotified)
        {
            //set expiration to 5 minutes
            memoryCache.Set(cacheKey, true, DateTimeOffset.Now.AddMinutes(5));

            await notificationSender.SendNotificationsAsync(new NotificationDto()
            {
                Title = $"{status.Vehicle.Name} has left the Virtual Garage!",
                Body =
                    $"Virtual garage  \nleft at: {DateTimeConverterHelper.ConvertToCentralEuropeTimeZone(status.Received):HH:mm:ss}",
                Data = new()
            }, adminDevices);
        }
    }

    private async Task SendLowBatteryNotification(Domain.Entities.Status status, Domain.Entities.Status vehicleStatus,
        List<string> adminDevices)
    {
        string cacheKey = $"LowBatteryNotification:{vehicleStatus.VehicleId}";
        var wasAlreadyNotified = cacheService.GetBatteryNotified(cacheKey);

        var vehicle = await vehiclesRepository.GetById(vehicleStatus.VehicleId);

        var threshold = vehicle!.Settings.LowBatteryThreshold / 100.0;

        if (vehicleStatus.BatteryInfo.ChargeLevel <= threshold && !wasAlreadyNotified)
        {
            memoryCache.Set(cacheKey, true);
            await notificationSender.SendNotificationsAsync(new NotificationDto()
            {
                Title = $"{status.Vehicle.Name} battery below {vehicle!.Settings.LowBatteryThreshold} %",
                Body =
                    $"Battery Low \nLast time: {DateTimeConverterHelper.ConvertToCentralEuropeTimeZone(status.Received):HH:mm:ss}",
                Data = new Dictionary<string, string>()
                {
                    { "vehicleId", status.VehicleId },
                    { "deviceId", status.DeviceId },
                    { "statusId", status.Id }
                }
            }, adminDevices);
        }

        if (vehicleStatus.BatteryInfo.ChargeLevel > threshold && wasAlreadyNotified)
        {
            memoryCache.Set(cacheKey, false);
        }
    }

    private async Task<(Domain.Entities.Status, string? lastNamedLocation)> CreateNewOrUpdateExistingStatus(
        Domain.Entities.Status status)
    {
        Domain.Entities.Status vehicleStatusForCache;
        string? lastnamedLocation = null;
        var lastKnownStatus = cacheService.GetLastKnownStatusFromCache(status.DeviceId);
        if (lastKnownStatus is null)
        {
            context.Statuses.Add(status);
            vehicleStatusForCache = status;
        }
        else
        {
            //Zjisit jestli je potřeba nový nebo ne a podle toho updatovat
            if (!LocationHasChanged(lastKnownStatus.Location, status.Location))
            {
                var oldStatus = await context.Statuses
                    .Include(x => x.Location)
                    .Include(x => x.BatteryInfo)
                    .FirstOrDefaultAsync(x => x.Id == lastKnownStatus.Id);

                lastnamedLocation = lastKnownStatus.NamedVehicleLocationId;

                oldStatus = UpdateStatus(oldStatus, status);

                CheckAndDisableVirtualGarageValidity(oldStatus);

                context.Update(oldStatus);

                vehicleStatusForCache = oldStatus;
            }
            else
            {
                //pokud je lokace jiná, tak uložit nový status
                CheckAndDisableVirtualGarageValidity(status);
                context.Statuses.Add(status);
                vehicleStatusForCache = status;
                lastnamedLocation = lastKnownStatus.NamedVehicleLocationId;
            }
        }

        await context.SaveChangesAsync();

        return (vehicleStatusForCache, lastnamedLocation);
    }

    private static void CheckAndDisableVirtualGarageValidity(Domain.Entities.Status status)
    {
        if (status.Vehicle.VirtualGarageIsEnabled &&
            status.Vehicle.VirtualGarage.ValidTill < DateTime.Now)
        {
            status.Vehicle.VirtualGarageIsEnabled = false;
        }
    }


    private bool LocationHasChanged(Location locationA, Location locationB)
    {
        var distance = GetDistanceInMeters(locationA.Latitude, locationA.Longitude, locationB.Latitude,
            locationB.Longitude);
        return distance > 50.0;
    }

    private double GetDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000.0; // Radius of Earth in meters

        // Convert degrees to radians
        double latRad1 = lat1 * Math.PI / 180.0;
        double lonRad1 = lon1 * Math.PI / 180.0;
        double latRad2 = lat2 * Math.PI / 180.0;
        double lonRad2 = lon2 * Math.PI / 180.0;

        // Differences
        double deltaLat = latRad2 - latRad1;
        double deltaLon = lonRad2 - lonRad1;

        // Haversine formula
        double a = Math.Sin(deltaLat / 2.0) * Math.Sin(deltaLat / 2.0) +
                   Math.Cos(latRad1) * Math.Cos(latRad2) *
                   Math.Sin(deltaLon / 2.0) * Math.Sin(deltaLon / 2.0);

        double c = 2.0 * Math.Asin(Math.Sqrt(a));

        // Distance in meters
        double distance = R * c;
        return distance;
    }

    private Domain.Entities.Status UpdateStatus(Domain.Entities.Status lastKnownStatus, Domain.Entities.Status status)
    {
        lastKnownStatus.BatteryInfo = status.BatteryInfo;
        lastKnownStatus.Location = status.Location;
        lastKnownStatus.Received = status.Received;
        lastKnownStatus.Sent = status.Sent;
        return lastKnownStatus;
    }

    private void CreateCacheEntry(Domain.Entities.Status status, List<string> adminNotificationTokens)
    {
        var expirationToken = new CancellationChangeToken(
            new CancellationTokenSource(TimeSpan.FromSeconds(cacheOptions.Value.DefaultExpiration + 1)).Token);
        memoryCache.Set(status.DeviceId, status, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, cacheOptions.Value.DefaultExpiration),
            PostEvictionCallbacks =
            {
                new PostEvictionCallbackRegistration
                {
                    EvictionCallback = async (key, value, reason, state) =>
                    {
                        if (reason == EvictionReason.Expired)
                        {
                            Console.WriteLine(
                                $"Expired_________________ {status.Vehicle.Name} ____ vehicleId: {status.Vehicle.Id}");
                            await notificationSender.SendNotificationsAsync(
                                new NotificationDto()
                                {
                                    Title = $"{status.Vehicle.Name} status expired",
                                    Body =
                                        $"Status expired \nLast time: {DateTimeConverterHelper.ConvertToCentralEuropeTimeZone(status.Received):HH:mm:ss}",
                                    Data = new Dictionary<string, string>()
                                    {
                                        { "vehicleId", status.VehicleId },
                                        { "deviceId", status.DeviceId },
                                        { "statusId", status.Id }
                                    }
                                }, adminNotificationTokens
                            );
                        }
                    }
                },
            }
        });
    }
}