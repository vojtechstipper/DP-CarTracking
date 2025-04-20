using System.Text;
using CarTracking.BE.Application.Repositories;
using CarTracking.BE.Application.Services;
using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;

namespace CarTracking.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<CarTrackingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database"),
                o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "CarTrackingData")));

        services.AddDbContext<CarTrackingIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database"),
                o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "CarTrackingIdentity")));
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IVehiclesRepository, VehiclesRepository>();
        services.AddScoped<IDeviceRepository, DevicesRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IAccountsRepository, AccountsRepository>();
        services.AddScoped<IPasswordResetCodesRepository, PasswordResetCodesRepository>();
        services.AddScoped<IVehicleLocationsRepository, VehicleLocationsRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<StatusService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthentificaitonService, AuthentificationService>();
        services.AddScoped<INotificationSender, FirebaseNotificationSender>();
        services.AddScoped<IEmailSender, SendGridEmailSender>();

        return services;
    } 
    
    public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureSwaggerGen(options => { options.CustomSchemaIds(x => x.FullName); });

        return services;
    }

    public static IServiceCollection AddJwtToken(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "your_issuer",
                    ValidAudience = "your_audience",
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("your_secret_key_your_secret_key_your_secret_key"))
                };
            });

        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<CarTrackingIdentityDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);;

        return services;
    }
}