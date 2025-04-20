using CarTracking.BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarTracking.BE.Infrastructure.Configurations;

public class VehicleLocationConfiguration: IEntityTypeConfiguration<VehicleLocation>
{
    public void Configure(EntityTypeBuilder<VehicleLocation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.VehicleLocations)
            .HasForeignKey(x => x.VehicleId);

        builder.HasIndex(x => x.VehicleId);
    }
}