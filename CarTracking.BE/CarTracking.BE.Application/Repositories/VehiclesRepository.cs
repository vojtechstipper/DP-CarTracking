using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public interface IVehiclesRepository : IRepository<Vehicle>
{
    Task<ICollection<Vehicle>> GetVehiclesForAccount(string accountId);
    Task<Vehicle?> GetVehicleByDeviceId(string deviceId);
    Task<List<Vehicle>> GetAllVehicles();
}

public class VehiclesRepository(CarTrackingDbContext context) : Repository<Vehicle>(context), IVehiclesRepository
{
    public Task<ICollection<Vehicle>> GetVehiclesForAccount(string accountId)
    {
        throw new NotImplementedException();
    }

    public async Task<Vehicle?> GetVehicleByDeviceId(string deviceId)
    {
        return await context.Vehicles.Include(x => x.Device)
            .Where(x => x.Device != null)
            .FirstOrDefaultAsync(x => x.Device != null && x.Device.Id == deviceId);
    }

    public async Task<List<Vehicle>> GetAllVehicles()
    {
        return await context.Vehicles.Include(x => x.Settings).ToListAsync();
    }
}