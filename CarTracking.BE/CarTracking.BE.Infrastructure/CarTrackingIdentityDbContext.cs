using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Infrastructure;

public class CarTrackingIdentityDbContext(DbContextOptions<CarTrackingIdentityDbContext> options)
    : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("CarTrackingIdentity");
    }
}