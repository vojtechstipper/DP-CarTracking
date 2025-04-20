using CarTracking.BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarTracking.BE.Infrastructure.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Settings);
        
        builder.OwnsOne(x => x.VirtualGarage);

        builder.HasMany(x => x.StatusHistory)
            .WithOne(x => x.Vehicle)
            .HasForeignKey(x => x.VehicleId); 
        
        builder.HasMany(x => x.VehicleLocations)
            .WithOne(x => x.Vehicle)
            .HasForeignKey(x => x.VehicleId);

        builder.HasOne(x => x.Device)
            .WithOne(x => x.Vehicle)
            .HasForeignKey<Device>(x => x.VehicleId)
            .IsRequired(false);
    }
}