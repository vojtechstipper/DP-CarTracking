using CarTracking.BE.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.API;

public static class AppBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CarTrackingDbContext>();
            dbContext.Database.Migrate();
        }
        
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CarTrackingIdentityDbContext>();
            dbContext.Database.Migrate();
        }
    }
}