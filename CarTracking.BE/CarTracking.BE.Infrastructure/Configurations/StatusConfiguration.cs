using CarTracking.BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarTracking.BE.Infrastructure.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.VehicleId);

        builder.OwnsOne(x => x.Location);
        builder.OwnsOne(x => x.BatteryInfo);

        builder.HasIndex(x => x.VehicleId);
    }
}