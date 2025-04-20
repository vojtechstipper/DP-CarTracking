using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.ViewModels;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace CarTracking.MobileApp.Pages;

[QueryProperty(nameof(Vehicle), "Vehicle")]
public partial class VehicleStatusDetailPage : ContentPage
{
    public string Vehicle
    {
        set => _modelView.VehicleId = value;
    }

    private readonly VehicleStatusDetailModelView _modelView;

    public VehicleStatusDetailPage(VehicleStatusDetailModelView modelView)
    {
        _modelView = modelView;
        map = new Map();
        BindingContext = _modelView;
        InitializeComponent();
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        map.Pins.Clear();
        await _modelView.LoadVehicleLastStatus();
        var pin = _modelView.LastStatus.Location.CreatePinForMap(_modelView.LastStatus.VehicleName);
        map.AddPinToMap(pin);
        map.MoveToPinLocation(pin);
    }

    private async void OnActivateVirtualGarageClicked(object? sender, EventArgs e)
    {
        await _modelView.ActivateVirtualGarage();
    }

    private async void OnDeactivateVirtualGarageClicked(object? sender, EventArgs e)
    {
        await _modelView.DeactivateVirtualGarage();
    }
}