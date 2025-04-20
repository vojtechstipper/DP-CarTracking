using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace CarTracking.MobileApp.Pages;

[QueryProperty(nameof(VehicleId), "VehicleId")]
[QueryProperty(nameof(VehicleLocation), "VehicleLocation")]
public partial class LocationSelectorPage : ContentPage
{
    public string VehicleId
    {
        set => _modelView.VehicleId = value;
    }

    public VehicleLocationDto VehicleLocation
    {
        set { _modelView.EditLocation = value; }
    }

    private void LoadFromEditLocation()
    {
        if (_modelView.EditLocation != null)
        {
            LocationName.Text = _modelView.EditLocation.Name;
            _currentRadius = _modelView.EditLocation.Radius;
            RadiusSlider.Value = _currentRadius;
            RadiusLabel.Text = $"{_currentRadius:F0}m";
            UseAsVirtualFenceSwitch.IsToggled = _modelView.EditLocation.IsUsedForVirtualGarage;
            FromTime.Time = _modelView.EditLocation.FromTime;
            ToTime.Time = _modelView.EditLocation.ToTime;
            _selectedPin = new Pin
            {
                Label = "Selected Location",
                Location = new Location(_modelView.EditLocation.Location.Latitude,
                    _modelView.EditLocation.Location.Longitude),
                Type = PinType.Place
            };
            GoogleMap.Pins.Add(_selectedPin);
            UpdateRadiusCircle(_selectedPin.Location);
            var location = new Location(_modelView.EditLocation.Location.Latitude,
                _modelView.EditLocation.Location.Longitude);
            GoogleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude),
                Distance.FromMiles(1)));
        }
    }

    private LocationSelectorModelView _modelView;

    private Pin _selectedPin;
    private Circle _radiusCircle;
    private double _currentRadius = 200; // Default radius in meters

    public LocationSelectorPage(LocationSelectorModelView modelView)
    {
        InitializeComponent();
        _modelView = modelView;
        Appearing += OnAppearing;
    }

    private async void OnAppearing(object? sender, EventArgs e)
    {
        LocationName.IsEnabled = true;
        GoogleMap.Pins.Clear();
        LocationName.Text = "";
        _currentRadius = 200;
        RadiusSlider.Value = _currentRadius;
        UseAsVirtualFenceSwitch.IsToggled = false;
        GridTime.IsVisible = false;
        FromTime.Time = new TimeSpan(0, 0, 0);
        ToTime.Time = new TimeSpan(23, 59, 59);
        GoogleMap.MapElements.Clear();

        if (_modelView.EditLocation != null)
        {
            LoadFromEditLocation();
            AddLocationButton.Text = "Update";
        }
        else
        {
            AddLocationButton.Text = "Add";
            await _modelView.LoadVehicleLastLocation();

            var location = _modelView.LastLocation;
            if (location is not null)
                GoogleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude),
                    Distance.FromMiles(1)));
        }
    }

    private void OnRadiusChanged(object? sender, ValueChangedEventArgs e)
    {
        _currentRadius = e.NewValue;
        RadiusLabel.Text = $"{_currentRadius:F0}m";

        if (_selectedPin != null)
        {
            UpdateRadiusCircle(_selectedPin.Location);
        }
    }

    private void OnMapClicked(object? sender, MapClickedEventArgs e)
    {
        var position = e.Location;

        // Remove previous pin if exists
        GoogleMap.Pins.Clear();

        // Create a new pin
        _selectedPin = new Pin
        {
            Label = "Selected Location",
            Location = position,
            Type = PinType.Place
        };

        GoogleMap.Pins.Add(_selectedPin);

        // Draw the radius circle
        UpdateRadiusCircle(position);
    }

    private void UpdateRadiusCircle(Location position)
    {
        // Remove previous circle if exists
        GoogleMap.MapElements.Clear();

        // Create a new circle
        _radiusCircle = new Circle
        {
            Center = position,
            Radius = Distance.FromMeters(_currentRadius),
            StrokeColor = Color.FromRgba(255, 0, 0, 128), // Red with transparency
            FillColor = Color.FromRgba(255, 0, 0, 64),
            StrokeWidth = 2
        };

        GoogleMap.MapElements.Add(_radiusCircle);
    }

    private async void OnAddLocationButtonClicked(object? sender, EventArgs e)
    {
        LocationName.IsEnabled = false;
        if (_selectedPin is not null)
        {
            if (_modelView.EditLocation is not null)
            {
                var result = await _modelView.UpdateLocation(_selectedPin.Location, _currentRadius, LocationName.Text,
                    _modelView.EditLocation!.Id, UseAsVirtualFenceSwitch.IsToggled, FromTime.Time, ToTime.Time);
                if (result.IsSuccess)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Update Location Error", "Could not update location, try again", "OK");
                }
            }
            else
            {
                var result = await _modelView.AddLocation(_selectedPin.Location, _currentRadius, LocationName.Text,
                    UseAsVirtualFenceSwitch.IsToggled, FromTime.Time, ToTime.Time);
                if (result.IsSuccess)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Add Location Error", "Could not add location, try again", "OK");
                }
            }
        }
    }

    private async void OnLoadLocationButtonClicked(object? sender, EventArgs e)
    {
        try
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            Location location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
            {
                GoogleMap.Pins.Clear();
                GoogleMap.MapElements.Clear();
                GoogleMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude),
                    Distance.FromMiles(1)));
                _selectedPin = new Pin
                {
                    Label = "Current Location",
                    Location = location,
                    Type = PinType.Place
                };
                GoogleMap.Pins.Add(_selectedPin);

                // Draw the radius circle
                UpdateRadiusCircle(location);
            }
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
    }

    private void OnUseAsVirtualFenceSwitchToggled(object? sender, ToggledEventArgs e)
    {
        GridTime.IsEnabled = e.Value;
        GridTime.IsVisible = e.Value;
    }
}