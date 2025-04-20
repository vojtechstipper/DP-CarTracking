using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoMapper;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Services;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace CarTracking.MobileApp.ViewModels;

public class LocationSelectorModelView : INotifyPropertyChanged
{
    public string VehicleId { get; set; }
    public VehicleLocationDto EditLocation { get; set; }
    private readonly VehicleService _vehicleService;
    private readonly IMapper _mapper;
    private LocationDto _lastLocation;

    public LocationDto LastLocation
    {
        get => _lastLocation;
        set
        {
            if (_lastLocation != value)
            {
                _lastLocation = value;
                OnPropertyChanged(nameof(LastLocation));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public LocationSelectorModelView(VehicleService vehicleService, IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    public async Task LoadVehicleLastLocation()
    {
        var result = await _vehicleService.GetVehicleLastStatus(VehicleId);
        if (result.IsSuccess && result.Value != null)
        {
            LastLocation = result.Value.Location;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task<Result<string>> AddLocation(Location selectedPinLocation, double currentRadius, string name,
        bool isUsedForVirtualGarage, TimeSpan fromTime, TimeSpan toTime)
    {
        return await _vehicleService.AddLocationToVehicle(VehicleId,
            new LocationDto() { Latitude = selectedPinLocation.Latitude, Longitude = selectedPinLocation.Longitude },
            currentRadius, name, isUsedForVirtualGarage, fromTime, toTime);
    }


    public async Task<Result<string>> UpdateLocation(Location selectedPinLocation, double currentRadius,
        string locationNameText, string id, bool isUsedForVirtualGarage, TimeSpan fromTime, TimeSpan toTime)
    {
        return await _vehicleService.UpdateLocationToVehicle(VehicleId,
            new LocationDto() { Latitude = selectedPinLocation.Latitude, Longitude = selectedPinLocation.Longitude },
            currentRadius, locationNameText, id, isUsedForVirtualGarage, fromTime, toTime);
    }
}