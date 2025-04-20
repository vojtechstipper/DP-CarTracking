using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Firebase.CloudMessaging;
#if ANDROID
using Android.Content;
#endif

namespace CarTracking.MobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity ActivityCurrent { get; set; } = null!;

    public MainActivity()
    {
        ActivityCurrent = this;
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (Intent != null) HandleIntent(Intent);
        CreateNotificationChannelIfNeeded();
        // Set current activity when OnCreate is called
        ActivityCurrent = this;

        var mode = Preferences.Get("Mode", "");
        if (mode == "Monitor")
        {
            // Call Start() after ActivityCurrent is set
            StartBackgroundService();
        }
        
    }
    private static void HandleIntent(Intent intent)
    {
        FirebaseCloudMessagingImplementation.OnNewIntent(intent);
    }
    private void StartBackgroundService()
    {
        Start();
    }

    public void Start()
    {
        Intent startService = new Intent(ActivityCurrent, typeof(SendInfoService));
        startService.SetAction("START_SERVICE");
        ActivityCurrent.StartService(startService);
    }
    private void CreateNotificationChannelIfNeeded()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            CreateNotificationChannel();
        }
    }
    private void CreateNotificationChannel()
    {
        var channelId = $"{PackageName}.general";
        var notificationManager = (NotificationManager)GetSystemService(NotificationService)!;
        var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
        notificationManager.CreateNotificationChannel(channel);
        FirebaseCloudMessagingImplementation.ChannelId = channelId;
    }
}