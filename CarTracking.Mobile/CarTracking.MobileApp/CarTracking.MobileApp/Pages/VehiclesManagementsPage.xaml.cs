using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

public partial class VehiclesManagementsPage : ContentPage
{
    private readonly VehiclesManagementModelView _modelView;

    public VehiclesManagementsPage(VehiclesManagementModelView modelView)
    {
        InitializeComponent();
        _modelView = modelView;
        BindingContext = _modelView;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IsEnabled = false, // Disable back button
            IsVisible = false // Hide back button
        });
        Shell.SetNavBarIsVisible(this, false);
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        await _modelView.LoadVehicles();
    }

    private async void OnAddNewVehicleClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(CreateVehiclePage)}");
    }

    private async void OnEditClicked(object? sender, EventArgs e)
    {
        var vehicle = (sender as Button)?.CommandParameter as VehicleDto;
        await Shell.Current.GoToAsync($"/{nameof(VehicleSettingsPage)}",
            new Dictionary<string, object> { { "VehicleId", vehicle.Id } });
    }

    private void OnMenuButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
    }
}