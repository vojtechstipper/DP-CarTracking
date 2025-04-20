using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

[QueryProperty(nameof(Vehicle), "Vehicle")]
public partial class VehicleTripsPage : ContentPage
{
    private readonly VehicleTripsModelView _modelView;

    public string Vehicle
    {
        set => _modelView.VehicleId = value;
    }

    public VehicleTripsPage(VehicleTripsModelView modelView)
    {
        _modelView = modelView;
        InitializeComponent();
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        BindingContext = _modelView;
        _modelView.IsBusy = true;
        await _modelView.LoadTrips();
        _modelView.IsBusy = false;
    }

    private async void OnShowOnMapClicked(object? sender, EventArgs e)
    {
        var vehicleTripInfo = (sender as Button)?.CommandParameter as VehicleTripInfoListItem;
        if (vehicleTripInfo is not null)
        {
            await Shell.Current.GoToAsync($"/{nameof(TripMapPage)}",
                new Dictionary<string, object>()
                    { { "Locations", vehicleTripInfo.Locations } });
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        var vehicleTripInfo = (sender as Button)?.CommandParameter as VehicleTripInfoListItem;
        if (vehicleTripInfo is not null)
        {
            bool answer = await DisplayAlert("Remove Trip",
                "You will delete all historical statuses contained it this trip, are you sure?", "Yes", "No");
            if (answer && vehicleTripInfo.TripId is not null)
            {
                await _modelView.DeleteTrip(vehicleTripInfo.TripId);
            }
        }
    }

    private async void OnGenerateTripsClicked(object? sender, EventArgs e)
    {
        _modelView.IsBusy = true;
        await _modelView.GenerateTrips();
        _modelView.IsBusy = false;
    }
}