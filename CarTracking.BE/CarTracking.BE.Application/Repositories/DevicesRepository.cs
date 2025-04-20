using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<ICollection<Device>> GetAdminDevicesForAccount(string accountId);
    Task<Device?> GetAdminDeviceByAccountAndToken(string accountId, string requestNotificationToken);
    Task<Device?> GetDeviceByIdWithVehicle(string deviceId);
}

public class DevicesRepository(CarTrackingDbContext context) : Repository<Device>(context), IDeviceRepository
{
    public async Task<ICollection<Device>> GetAdminDevicesForAccount(string accountId)
    {
        return await context.Devices.Where(x => x.AccountId == accountId).ToListAsync();
    }

    public async Task<Device?> GetAdminDeviceByAccountAndToken(string accountId, string requestNotificationToken)
    {
        return await context.Devices.FirstOrDefaultAsync(x =>
            x.AccountId == accountId &&
            x.NotificationToken == requestNotificationToken &&
            x.IsAdminDevice);
    }

    public async Task<Device?> GetDeviceByIdWithVehicle(string deviceId)
    {
        return await context.Devices
            .Include(x => x.Vehicle)
            .FirstOrDefaultAsync(x => x.Id == deviceId);
    }
}