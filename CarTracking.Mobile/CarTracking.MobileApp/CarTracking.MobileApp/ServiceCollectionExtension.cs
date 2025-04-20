using CarTracking.MobileApp.API;
using CarTracking.MobileApp.Pages;
using Refit;

namespace CarTracking.MobileApp;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPages(this IServiceCollection services)
    {
        services.AddSingleton<MainPage>();
        services.AddSingleton<VehicleSelectPage>();
        services.AddSingleton<ModeSelectPage>();
        services.AddSingleton<MonitorModeHomePage>();
        services.AddSingleton<AdminModeHomePage>();
        services.AddSingleton<VehicleStatusDetailPage>();
        services.AddSingleton<VehicleTripsPage>();
        services.AddSingleton<UserLoginPage>();
        services.AddSingleton<UserRegisterPage>();
        services.AddSingleton<AccountAssigningPage>();
        services.AddSingleton<CreateVehiclePage>();
        services.AddSingleton<AccountCodeGeneratorPage>();
        services.AddSingleton<TripMapPage>();
        services.AddSingleton<VehiclesManagementsPage>();
        services.AddSingleton<VehicleSettingsPage>();
        services.AddSingleton<UserPasswordResetPage>();
        services.AddSingleton<LocationSelectorPage>();
        
        return services;
    }

    public static IServiceCollection AddCarTrackingApi(this IServiceCollection services)
    {
        services.AddRefitClient<ICarTrackingApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.URL))
            .AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddRefitClient<ICarTrackingApiUnathorized>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.URL));
        services.AddSingleton(typeof(ApiWrapper<>));
        services.AddTransient<AuthHeaderHandler>();
        return services;
    }
}