using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Services;
using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

public partial class MonitorModeHomePage : ContentPage
{
    private readonly VehicleService _vehicleService;
    private readonly DeviceService _deviceService;

    public MonitorModeHomePage(VehicleService vehicleService, DeviceService deviceService)
    {
        InitializeComponent();
        _vehicleService = vehicleService;
        _deviceService = deviceService;
        Appearing += OnAppearing;
        BindingContext = LogViewModel.Instance;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        Shell.SetNavBarIsVisible(this, false);
        await TryGetVehicleName();
    }

    private async Task TryGetVehicleName()
    {
        var vehicleId = await SecureStorageAccessor.GetVehicelId();
        if (string.IsNullOrEmpty(vehicleId)) return;
        var vehicleConfig = await _vehicleService.GetVehicleConfig(vehicleId);
        if (vehicleConfig?.Name is not null)
        {
            VehicleName.Text = vehicleConfig.Name;
        }
    }

    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        if (sender is null) return;
        var val = (int)((sender as Slider)!).Value;
        LogViewModel.Instance.UpdateKeepCount(val);
    }

    private void OnMenuButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
    }

    private async void OnSendTestNotificationsClicked(object? sender, EventArgs e)
    {
        var result = await _deviceService.SendTestNotification();
        LogViewModel.Instance.AddTestNotificationLog(result.IsSuccess
            ? new Log { Date = DateTime.Now, Text = "Ok" }
            : new Log { Date = DateTime.Now, Text = "Failed" });
    }
}