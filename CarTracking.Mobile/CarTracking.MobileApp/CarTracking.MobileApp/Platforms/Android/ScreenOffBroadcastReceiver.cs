using Android.App;
using Android.Content;
using Android.OS;
using Microsoft.Extensions.Logging;

namespace CarTracking.MobileApp;

[BroadcastReceiver(Name = "CarTracking.MobileApp.ScreenOffBroadcastReceiver", Label = "ScreenOffBroadcastReceiver",
    Exported = true)]
[IntentFilter(new[] { Intent.ActionScreenOff }, Priority = (int)IntentFilterPriority.HighPriority)]
public class ScreenOffBroadcastReceiver : BroadcastReceiver
{
    private readonly ILogger<ScreenOffBroadcastReceiver> _logger;

    private PowerManager.WakeLock _wakeLock;

    public ScreenOffBroadcastReceiver()
    {
        _logger = Microsoft.Maui.Controls.Application.Current?.Handler?.MauiContext?.Services
            .GetService<ILogger<ScreenOffBroadcastReceiver>>() ?? throw new InvalidOperationException();
    }

    public override void OnReceive(Context? context, Intent? intent)
    {
        if (intent != null && intent.Action == Intent.ActionScreenOff)
        {
            AcquireWakeLock();
        }
    }

    private void AcquireWakeLock()
    {
        _wakeLock?.Release();

        WakeLockFlags wakeFlags = WakeLockFlags.Partial;

        PowerManager pm = (PowerManager)Android.App.Application.Context.GetSystemService(Context.PowerService);
        _wakeLock = pm.NewWakeLock(wakeFlags, typeof(ScreenOffBroadcastReceiver).FullName);
        _wakeLock.Acquire();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _wakeLock?.Release();
    }
}