using Android.Content;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

public partial class VehicleSelectPage : ContentPage
{
    private readonly VehicleSelectModelView _modelView;

    public VehicleSelectPage(VehicleSelectModelView modelView)
    {
        InitializeComponent();
        _modelView = modelView;
        Appearing += OnAppearing;
        BindingContext = _modelView;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        await _modelView.LoadVehicles();
    }

    private async void OnVehicleSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var selectedVehicle = e.SelectedItem as VehicleDto;

        if (selectedVehicle is null) return;

        if (selectedVehicle.IsAssigned && !string.IsNullOrEmpty(selectedVehicle.DeviceId))
        {
            Preferences.Set("DeviceId", selectedVehicle.DeviceId);
            await SecureStorageAccessor.SetDeviceId(selectedVehicle.DeviceId);
            Preferences.Set("VehicleId", selectedVehicle.Id);
            await SecureStorageAccessor.SetVehicleId(selectedVehicle.Id);
            Preferences.Set("Mode", "Monitor");
            Preferences.Set("IsModeChoosen", true);
        }
        else
        {
            await _modelView.RegisterVehicleAndSaveToPreferences(selectedVehicle);
        }

        StartBackgroundServiceSendInfo();

        ((ListView)sender!).SelectedItem = null;
        await Shell.Current.GoToAsync($"/{nameof(MonitorModeHomePage)}");
    }

    private static void StartBackgroundServiceSendInfo()
    {
        Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(SendInfoService));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }
}