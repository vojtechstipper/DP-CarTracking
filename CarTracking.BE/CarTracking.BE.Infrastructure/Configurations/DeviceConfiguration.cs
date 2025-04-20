using CarTracking.BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarTracking.BE.Infrastructure.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Account)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.AccountId);

        builder.HasOne(x => x.Vehicle)
            .WithOne(x => x.Device)
            .HasForeignKey<Vehicle>(x => x.DeviceId)
            .IsRequired(false);
    }
}