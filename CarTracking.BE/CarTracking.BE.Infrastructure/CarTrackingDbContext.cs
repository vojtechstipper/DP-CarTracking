using System.Reflection;
using CarTracking.BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Infrastructure;

public class CarTrackingDbContext(DbContextOptions<CarTrackingDbContext> options) : DbContext(options)
{
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<PasswordResetCode> PasswordResetCodes => Set<PasswordResetCode>();
    public DbSet<VehicleLocation> VehicleLocations => Set<VehicleLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("CarTrackingData");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}