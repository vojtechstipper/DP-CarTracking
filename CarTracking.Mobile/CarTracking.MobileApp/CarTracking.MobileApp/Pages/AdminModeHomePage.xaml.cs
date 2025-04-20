using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.ViewModels;
using Microsoft.Maui.Maps;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace CarTracking.MobileApp.Pages;

public partial class AdminModeHomePage : ContentPage
{
    private readonly AdminHomePageModelView _view;

    public AdminModeHomePage(AdminHomePageModelView view)
    {
        _view = view;
        map = new Map();
        InitializeComponent();
        Appearing += OnAppearing;
        _view.RefreshCompleted += OnRefreshCompleted;
    }

    private void OnRefreshCompleted(object sender, EventArgs e)
    {
        map.Pins.Clear();
        UpdateMapPins();
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        Shell.SetNavBarIsVisible(this, false);
        map.Pins.Clear();
        await _view.LoadVehicles();
        UpdateMapPins();

        BindingContext = _view;
    }

    private void UpdateMapPins()
    {
        var vehiclesWithLocation = _view.Vehicles.Where(x => x.LastKnownLocation != null).ToList();
        vehiclesWithLocation.ForEach(x => map.AddPinToMap(x.LastKnownLocation!.CreatePinForMap(x.Name)));

        if (vehiclesWithLocation.Count > 0)
        {
            FindAndMoveToCentroid(vehiclesWithLocation);
        }
    }

    private void FindAndMoveToCentroid(List<VehicleInfo> vehiclesWithLocation)
    {
        var centroidLatitude = vehiclesWithLocation.Average(x => x.LastKnownLocation!.Latitude);
        var centroidLongitude = vehiclesWithLocation.Average(x => x.LastKnownLocation!.Longitude);
        var radius = vehiclesWithLocation.Max(x => x.LastKnownLocation!.Latitude) -
                     vehiclesWithLocation.Min(x => x.LastKnownLocation!.Latitude);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(centroidLatitude, centroidLongitude),
            Distance.FromMiles(Math.Max(radius, 0.1) * 69)));
    }

    private void OnRemoveAllPreferencesClicked(object? sender, EventArgs e)
    {
        Preferences.Clear();
    }

    private void OnShowMapClicked(object? sender, EventArgs e)
    {
        var vehicle = GetVehicleFromSender(sender);
        if (vehicle?.LastKnownLocation != null)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Location(vehicle.LastKnownLocation.Latitude, vehicle.LastKnownLocation.Longitude),
                Distance.FromMiles(0.1)));
        }
    }

    private async void OnStatusDetailClicked(object? sender, EventArgs e)
    {
        await HandleNavigationAsync(sender, "/VehicleStatusDetailPage", "Vehicle Unavailable",
            " has no historical locations.");
    }

    private async void OnTripsClicked(object? sender, EventArgs e)
    {
        await HandleNavigationAsync(sender, "/VehicleTripsPage", "Vehicle Unavailable",
            " has no historical locations.");
    }

    private VehicleInfo? GetVehicleFromSender(object? sender)
    {
        return (sender as Button)?.CommandParameter as VehicleInfo;
    }

    private async Task HandleNavigationAsync(object? sender, string route, string title, string message)
    {
        var vehicle = GetVehicleFromSender(sender);
        if (vehicle?.LastKnownLocation is null)
        {
            await DisplayAlert(title, $"{vehicle?.Name}{message}", "OK");
        }
        else
        {
            await Shell.Current.GoToAsync(route, new Dictionary<string, object> { { "Vehicle", vehicle.Id } });
        }
    }

    private async void OnGenerateCodeButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(AccountCodeGeneratorPage)}");
    }

    private void OnMenuButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
    }
}