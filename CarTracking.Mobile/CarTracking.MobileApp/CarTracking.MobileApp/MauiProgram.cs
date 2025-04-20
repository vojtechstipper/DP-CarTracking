#if ANDROID
using CarTracking.MobileApp.Services;
#endif
using CarTracking.MobileApp.Profiles;
using CarTracking.MobileApp.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Platforms.Android;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Crashlytics;
using Shiny;

namespace CarTracking.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseShiny()
            .UseMauiMaps()
            .RegisterFirebaseServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

        builder.Services.AddCarTrackingApi();
        builder.Services.AddAutoMapper(cfg => cfg.AddProfile<VehicleInfoProfile>());

        builder.Services.AddPages();

#if ANDROID
        builder.Services.AddTransient<IServiceTest, SendInfoService>();
        builder.Services.AddTransient<ScreenOffBroadcastReceiver>();
        builder.Services.AddTransient<ScreenOffService>();

        builder.Services.AddTransient<DeviceService>();
        builder.Services.AddTransient<VehicleService>();
        builder.Services.AddTransient<UserService>();

        builder.Services.AddTransient<AdminHomePageModelView>();
        builder.Services.AddTransient<VehicleStatusDetailModelView>();
        builder.Services.AddTransient<VehicleTripsModelView>();
        builder.Services.AddTransient<UserLoginViewModel>();
        builder.Services.AddTransient<VehiclesManagementModelView>();
        builder.Services.AddTransient<VehicleSettingsModelView>();
        builder.Services.AddTransient<PasswordResetViewModel>();
        builder.Services.AddTransient<VehicleSelectModelView>();
        builder.Services.AddTransient<LocationSelectorModelView>();
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddAndroid(android => android.OnCreate((activity, _) =>
                CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));
            CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
        });

        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
        return builder;
    }

    private static CrossFirebaseSettings CreateCrossFirebaseSettings()
    {
        return new CrossFirebaseSettings(isAuthEnabled: true, isCloudMessagingEnabled: true, isAnalyticsEnabled: true);
    }
}