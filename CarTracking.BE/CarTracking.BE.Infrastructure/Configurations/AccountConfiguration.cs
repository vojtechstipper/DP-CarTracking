using CarTracking.BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarTracking.BE.Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Devices)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Vehicles)
            .WithOne(x => x.Account)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        // builder
        //     .Property(e => e.UserIds)
        //     .HasConversion(
        //         v => string.Join(',', v),
        //         v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
}