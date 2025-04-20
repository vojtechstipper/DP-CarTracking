using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CarTracking.MobileApp.API;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.ViewModels;
using Refit;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace CarTracking.MobileApp;

[Service(Name = "CarTracking.MobileApp.SendInfoService")]
public class SendInfoService : Service, IServiceTest
{
    private readonly ICarTrackingApiUnathorized _carTrackingApi =
        RestService.For<ICarTrackingApiUnathorized>(Constants.URL);

    Timer timer;

    public override IBinder? OnBind(Intent? intent)
    {
        throw new NotImplementedException();
    }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        if (intent != null && intent.Action == "START_SERVICE")
        {
            RegisterNotification(); //Proceed to notify
        }
        else if (intent != null && intent.Action == "STOP_SERVICE")
        {
            StopForeground(StopForegroundFlags.Remove);
            StopSelfResult(startId);
        }

        return StartCommandResult.NotSticky;
    }

    public void Start()
    {
        Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(SendInfoService));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
        stopIntent.SetAction("STOP_SERVICE");
        MainActivity.ActivityCurrent.StartService(stopIntent);
    }

    private void RegisterNotification()
    {
        NotificationChannel channel =
            new NotificationChannel("ServiceChannel", "StatusService", NotificationImportance.Max);
        NotificationManager manager =
            (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(NotificationService);
        manager.CreateNotificationChannel(channel);
        Notification notification = new Notification.Builder(this, "ServiceChannel")
            .SetContentTitle("CarTracking service is working")
            .SetSmallIcon(Resource.Mipmap.cartrackingicon)
            .SetOngoing(true)
            .Build();

        if ((int)Build.VERSION.SdkInt < 29)
        {
            StartForeground(100, notification);
        }
        else
        {
            StartForeground(100, notification,
                ForegroundService.TypeSpecialUse);
        }

        timer = new Timer(Timer_Elapsed, notification, 0, 30000);
    }

    private async Task SendStatus(Location location, BatteryInfoDto batteryStatus)
    {
        try
        {
            var deviceId = Preferences.Get("DeviceId", "NoId");
            if (deviceId != "NoId")
            {
                var response = await _carTrackingApi.SendStatus(new SendStatusDto()
                {
                    DeviceId = deviceId,
                    Status = "OK-bg",
                    Sent = DateTime.UtcNow,
                    Location = new SendStatusDto.LocationDto()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Speed = location.Speed,
                        Accuracy = location.Accuracy,
                        Altitude = location.Altitude
                    },
                    BatteryInfo = batteryStatus
                });
                if (response.IsSuccessful)
                    LogViewModel.Instance.AddLog(new Log { Date = DateTime.Now, Text = "OK" });
            }
        }
        catch (Exception e)
        {
            LogViewModel.Instance.AddLog(new Log { Date = DateTime.Now, Text = "Failed" });
            Console.WriteLine(e);
        }
    }

    private bool _isBatteryWatched;

    private BatteryInfoDto GetBatteryStatus()
    {
        return new()
        {
            ChargeLevel = Battery.Default.ChargeLevel,
            IsEnergySaverOn = Battery.Default.EnergySaverStatus == EnergySaverStatus.On,
            IsCharging = Battery.Default.PowerSource != BatteryPowerSource.Battery
        };
    }

    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    private async Task<Location?> TryGetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                return location;
            }

            _isCheckingLocation = false;
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }

        return null;
    }

    async void Timer_Elapsed(object state)
    {
        var location = await TryGetCurrentLocation();
        var batteryStatus = GetBatteryStatus();
        if (location is not null)
            await SendStatus(location, batteryStatus);
    }
}