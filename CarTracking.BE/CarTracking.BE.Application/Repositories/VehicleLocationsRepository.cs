using CarTracking.BE.Domain.Entities;
using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public interface IVehicleLocationsRepository : IRepository<VehicleLocation>
{
    Task<List<VehicleLocation>> GetVehicleLocationsByVehicleId(string vehicleId);
    Task<List<VehicleLocation>> GetVehicleLocationsForVirtualGarageByVehicleId(string vehicleId);
}

public class VehicleLocationsRepository(CarTrackingDbContext context)
    : Repository<VehicleLocation>(context), IVehicleLocationsRepository
{
    public async Task<List<VehicleLocation>> GetVehicleLocationsByVehicleId(string vehicleId)
    {
        return await context.VehicleLocations.Where(x => x.VehicleId == vehicleId).ToListAsync();
    }

    public async Task<List<VehicleLocation>> GetVehicleLocationsForVirtualGarageByVehicleId(string vehicleId)
    {
        return await context.VehicleLocations
            .Where(x => x.VehicleId == vehicleId)
            .Where(x=>x.IsUsedForVirtualGarage)
            .ToListAsync();
    }
}