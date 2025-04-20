using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.Pages;

public partial class CreateVehiclePage : ContentPage
{
    private readonly VehicleService _vehicleService;

    public CreateVehiclePage(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
        InitializeComponent();
        Appearing += OnAppearing;
        Disappearing += OnDisappearing;
    }

    private void OnDisappearing(object? sender, EventArgs e)
    {
        VehicleName.Text = "";
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        VehicleName.IsEnabled = true;
    }

    private async void AddVehicleButtonClicked(object? sender, EventArgs e)
    {
        string vehicleName = VehicleName.Text;
        var result = await _vehicleService.CreateNewVehicle(vehicleName);
        if (result.IsSuccess)
        {
            VehicleName.IsEnabled = false;
            await Shell.Current.GoToAsync($"/{nameof(VehiclesManagementsPage)}");
        }
        else

        {
            await DisplayAlert("Failed request", "Could not create vehicle, try again", "OK");
        }
    }
}