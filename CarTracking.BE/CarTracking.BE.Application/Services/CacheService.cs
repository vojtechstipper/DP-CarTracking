using Microsoft.Extensions.Caching.Memory;

namespace CarTracking.BE.Application.Services;

public interface ICacheService
{
    public Domain.Entities.Status? GetLastKnownStatusFromCache(string vehicleDeviceId);
    public bool GetBatteryNotified(string key);
    public bool GetVirtualGarageNotified(string key);
}

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public Domain.Entities.Status? GetLastKnownStatusFromCache(string vehicleDeviceId)
    {
        if (memoryCache.TryGetValue(vehicleDeviceId, out Domain.Entities.Status? status))
        {
            return status;
        }

        return null;
    }

    public bool GetBatteryNotified(string key)
    {
        if (memoryCache.TryGetValue(key, out bool wasNotified))
        {
            return wasNotified;
        }

        return false;
    }

    public bool GetVirtualGarageNotified(string key)
    {
        if (memoryCache.TryGetValue(key, out bool wasNotified))
        {
            return wasNotified;
        }

        return false;
    }
}