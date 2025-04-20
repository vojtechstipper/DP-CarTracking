using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;

namespace CarTracking.MobileApp;

[Service(Name = "CarTracking.MobileApp.ScreenOffService")]
public class ScreenOffService : Service
{
    private static readonly string TypeName = typeof(ScreenOffService).FullName!;
    public static readonly string ActionStartScreenOffService = TypeName + ".action.START";

    private const int NotificationId = 12345678;
    private const string NotificationChannelId = "screen_off_service_channel_01";
    private const string NotificationChannelName = "ScreenOffService";
    private NotificationManager _notificationManager;

    private bool _isStarted;

    private readonly ScreenOffBroadcastReceiver _screenOffBroadcastReceiver;

    public ScreenOffService()
    {
        _screenOffBroadcastReceiver =
            Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services
                .GetService<ScreenOffBroadcastReceiver>() ?? throw new InvalidOperationException();
    }

    public override void OnCreate()
    {
        base.OnCreate();

        _notificationManager = GetSystemService(Context.NotificationService) as NotificationManager ??
                               throw new InvalidOperationException();

        RegisterScreenOffBroadcastReceiver();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        UnregisterScreenOffBroadcastReceiver();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags,
        int startId)
    {
        CreateNotificationChannel(); // Elsewhere we must've prompted user to allow Notifications

        if (intent != null && intent.Action == ActionStartScreenOffService)
        {
            try
            {
                StartForeground();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to start Screen On/Off foreground svc: " + ex);
            }
        }

        return StartCommandResult.Sticky;
    }

    private void RegisterScreenOffBroadcastReceiver()
    {
        var filter = new IntentFilter();
        filter.AddAction(Intent.ActionScreenOff);
        RegisterReceiver(_screenOffBroadcastReceiver, filter);
    }

    private void UnregisterScreenOffBroadcastReceiver()
    {
        try
        {
            if (_screenOffBroadcastReceiver != null)
            {
                UnregisterReceiver(_screenOffBroadcastReceiver);
            }
        }
        catch (Java.Lang.IllegalArgumentException ex)
        {
            Console.WriteLine($"Error while unregistering {nameof(ScreenOffBroadcastReceiver)}. {ex}");
        }
    }

    private void StartForeground()
    {
        if (!_isStarted)
        {
            Notification notification = BuildInitialNotification();
            if ((int)Build.VERSION.SdkInt < 29)
            {
                StartForeground(NotificationId, notification);
            }
            else
            {
                StartForeground(NotificationId, notification,
                    ForegroundService.TypeSpecialUse);
            }

            _isStarted = true;
        }
    }

    private Notification BuildInitialNotification()
    {
        var intentToShowMainActivity = BuildIntentToShowMainActivity();

        var notification = new NotificationCompat.Builder(this, NotificationChannelId)
            .SetContentTitle("CarTracking")
            .SetContentText("ScreenOffService")
            .SetSmallIcon(Resource.Drawable
                .abc_ab_share_pack_mtrl_alpha) // Android top bar icon and Notification drawer item LHS icon
            .SetLargeIcon(global::Android.Graphics.BitmapFactory.DecodeResource(Resources,
                Resource.Drawable.abc_ab_share_pack_mtrl_alpha)) // Notification drawer item RHS icon
            .SetContentIntent(intentToShowMainActivity)
            .SetOngoing(true)
            .Build();

        return notification;
    }

    private PendingIntent BuildIntentToShowMainActivity()
    {
        var mainActivityIntent = new Intent(this, typeof(MainActivity));
        mainActivityIntent.SetAction("Constants.ACTION_MAIN_ACTIVITY");
        mainActivityIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
        mainActivityIntent.PutExtra("Constants.SERVICE_STARTED_KEY", true);

        PendingIntent pendingIntent =
            PendingIntent.GetActivity(this, 0, mainActivityIntent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable) ??
            throw new InvalidOperationException();

        return pendingIntent;
    }

    private void CreateNotificationChannel()
    {
        NotificationChannel chan = new(NotificationChannelId, NotificationChannelName, NotificationImportance.Default)
        {
            LightColor = Color.FromRgba(0, 0, 255, 0).ToInt(),
            LockscreenVisibility = NotificationVisibility.Public
        };

        _notificationManager.CreateNotificationChannel(chan);
    }

    public override IBinder OnBind(Intent? intent)
    {
        return null;
    }
}