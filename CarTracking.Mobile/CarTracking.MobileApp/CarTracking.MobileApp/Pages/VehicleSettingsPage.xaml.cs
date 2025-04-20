using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

[QueryProperty(nameof(VehicleId), "VehicleId")]
public partial class VehicleSettingsPage : ContentPage
{
    public string VehicleId
    {
        set => _modelView.VehicleId = value;
    }

    private readonly VehicleSettingsModelView _modelView;

    public VehicleSettingsPage(VehicleSettingsModelView modelView)
    {
        _modelView = modelView;
        InitializeComponent();
        BindingContext = _modelView;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        Task t1 = Task.Run(() => _modelView.LoadVehicleConfig());
        Task t2 = Task.Run(() => _modelView.LoadVehicleLocations());
        await Task.WhenAll(t1, t2);
    }

    private void Slider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        var val = (int)(sender as Slider).Value;
        _modelView.UpdateThreshold(val);
    }

    private void VirtualGarageRadiusSlider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        var val = (int)(sender as Slider).Value;
        _modelView.UpdateVirtualGarageRadius(val);
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        var result = await _modelView.Save();
        if (result.IsSuccess)
        {
            await Shell.Current.GoToAsync($"/{nameof(VehiclesManagementsPage)}");
        }
        else
        {
            await DisplayAlert("Edit Vehicle Error", "Could not edit vehicle, try again", "OK");
        }
    }

    private void HistoryInDaysSlider_OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        var val = (int)(sender as Slider).Value;
        _modelView.UpdateHistoryInDaysRadius(val);
    }

    private async void OnMapLocationClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(LocationSelectorPage)}",
            new Dictionary<string, object> { { "VehicleId", _modelView.VehicleId } });
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if ((sender as Button)?.CommandParameter is VehicleLocationDto vehicleLocation)
        {
            await _modelView.DeleteLocation(vehicleLocation.Id);
        }
    }

    private async void OnLocationEditClickd(object? sender, EventArgs e)
    {
        if ((sender as Button)?.CommandParameter is VehicleLocationDto vehicleLocation)
        {
            await Shell.Current.GoToAsync($"/{nameof(LocationSelectorPage)}",
                new Dictionary<string, object> { { "VehicleId", _modelView.VehicleId }, { "VehicleLocation", vehicleLocation } });
        }
    }
}