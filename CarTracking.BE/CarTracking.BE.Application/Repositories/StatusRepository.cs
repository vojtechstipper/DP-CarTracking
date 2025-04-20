using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public interface IStatusRepository : IRepository<Domain.Entities.Status>
{
    Task<Domain.Entities.Status?> GetLastStatusForVehicle(string vehicleId);
    IQueryable<Domain.Entities.Status> GetAllStatusesForVehicle(string vehicleId);
    Task<List<Domain.Entities.Status>> GetTripsForVehicle(string vehicleId);
    Task<List<Domain.Entities.Status>> GetStatusesForTrip(string requestVehicleId, string requestTripId);
}

public class StatusRepository(CarTrackingDbContext context)
    : Repository<Domain.Entities.Status>(context), IStatusRepository
{
    public async Task<Domain.Entities.Status?> GetLastStatusForVehicle(string vehicleId)
    {
        return await context.Statuses.Include(x => x.Vehicle)
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.Received)
            .FirstOrDefaultAsync();
    }

    public IQueryable<Domain.Entities.Status> GetAllStatusesForVehicle(string vehicleId)
    {
        return context.Statuses.Include(x => x.Vehicle)
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.Received)
            .AsQueryable();
    }

    public async Task<List<Domain.Entities.Status>> GetTripsForVehicle(string vehicleId)
    {
        return await context.Statuses
            .Where(status => status.TripId != null)
            .Where(status => status.VehicleId == vehicleId)
            .OrderBy(x => x.Received)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.Status>> GetStatusesForTrip(string requestVehicleId, string requestTripId)
    {
        return await context.Statuses
            .Where(x => x.VehicleId == requestVehicleId && x.TripId == requestTripId)
            .ToListAsync();
    }
}