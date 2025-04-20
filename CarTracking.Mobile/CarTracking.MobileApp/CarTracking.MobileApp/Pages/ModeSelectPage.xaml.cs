using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.Pages;

public partial class ModeSelectPage : ContentPage
{
    private readonly DeviceService _deviceService;

    public ModeSelectPage(DeviceService deviceService)
    {
        InitializeComponent();
        _deviceService = deviceService;
    }

    private async void OnMonitorModeClicked(object sender, EventArgs e)
    {
        if (!await CheckForLocationPermission())
        {
            await DisplayAlert("Access to location is required",
                "App needs access to location because it uses gps to track vehicles position", "OK");
        }

        await Shell.Current.GoToAsync($"/{nameof(VehicleSelectPage)}");
    }

    private async Task<bool> CheckForLocationPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
        if (status == PermissionStatus.Granted)
            return true;
        status = await Permissions.RequestAsync<Permissions.LocationAlways>();
        return status == PermissionStatus.Granted;
    }

    private async void OnAdminModeClicked(object sender, EventArgs e)
    {
        //zaregistrování zařízení
        var result = await _deviceService.RegisterAdminDevice();
        if (result.IsSuccess)
        {
            SetMode("Admin");
            await Shell.Current.GoToAsync($"/{nameof(AdminModeHomePage)}");
        }
        else
        {
            await DisplayAlert("Failed request", "Could not assign admin mode, try again", "OK");
        }
    }

    private static void SetMode(string mode)
    {
        Preferences.Set("IsModeChoosen", true);
        Preferences.Set("Mode", mode);
    }
}