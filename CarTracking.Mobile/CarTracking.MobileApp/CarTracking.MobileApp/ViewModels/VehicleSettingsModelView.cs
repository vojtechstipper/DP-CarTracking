using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class VehicleSettingsModelView(VehicleService vehicleService) : INotifyPropertyChanged
{
    public string VehicleId { get; set; }

    public VehicleConfig VehicleConfig
    {
        get => _VehicleConfig;
        set
        {
            _VehicleConfig = value;
            OnPropertyChanged();
        }
    }

    public List<VehicleLocationDto> VehicleLocations
    {
        get => _VehicleLocations;
        set
        {
            _VehicleLocations = value;
            OnPropertyChanged();
        }
    }

    private VehicleConfig _VehicleConfig { get; set; }

    private List<VehicleLocationDto> _VehicleLocations { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task LoadVehicleConfig()
    {
        VehicleConfig = await vehicleService.GetVehicleConfig(VehicleId);
    }

    public void UpdateThreshold(int val)
    {
        VehicleConfig.LowBatteryThreshold = val;
        OnPropertyChanged(nameof(VehicleConfig));
    }

    public void UpdateVirtualGarageRadius(int val)
    {
        if (val < 49) val = 50;
        VehicleConfig.VirtualGarageRadius = val;
        OnPropertyChanged(nameof(VehicleConfig));
    }

    public async Task<Result<string>> Save()
    {
        VehicleConfig.VehicleId = VehicleId;
        return await vehicleService.SaveEditedVehicle(VehicleConfig);
    }

    public void UpdateHistoryInDaysRadius(int val)
    {
        VehicleConfig.HistoryInDays = val;
        OnPropertyChanged(nameof(VehicleConfig));
    }

    public async Task LoadVehicleLocations()
    {
        var result = await vehicleService.GetVehicleLocations(VehicleId);
        if (result.IsSuccess && result.Value != null)
        {
            VehicleLocations = result.Value.Locations;
        }
    }

    public async Task DeleteLocation(string vehicleLocationId)
    {
        var result = await vehicleService.DeleteVehicleLocation(vehicleLocationId, VehicleId);
        if (result.IsSuccess)
        {
            VehicleLocations = VehicleLocations.Where(x => x.Id != vehicleLocationId).ToList();
            OnPropertyChanged(nameof(VehicleLocations));
        }
    }
}