using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class VehicleSelectModelView : INotifyPropertyChanged
{
    private readonly VehicleService _vehicleService;
    private readonly DeviceService _deviceService;
    public event PropertyChangedEventHandler? PropertyChanged;
    public List<VehicleDto> Vehicles { get; set; } = new();
    public bool IsVehicleListEmpty => !Vehicles.Any();
    public bool IsVehicleListNotEmpty => Vehicles.Any();

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public VehicleSelectModelView(VehicleService service, DeviceService deviceService)
    {
        _vehicleService = service;
        _deviceService = deviceService;
    }

    public async Task LoadVehicles()
    {
        Vehicles = await _vehicleService.GetVehicles();
        OnPropertyChanged(nameof(Vehicles));
        OnPropertyChanged(nameof(IsVehicleListEmpty));
        OnPropertyChanged(nameof(IsVehicleListNotEmpty));
    }

    public async Task RegisterVehicleAndSaveToPreferences(VehicleDto selectedVehicle)
    {
        var result = await _deviceService.RegisterMonitorDevice(selectedVehicle.Id);
        //TODO odstarnit z preferences
        if (result.IsSuccess)
        {
            Preferences.Set("DeviceId", result.Value!.DeviceId);
            await SecureStorageAccessor.SetDeviceId(result.Value.DeviceId);

            Preferences.Set("VehicleId", selectedVehicle.Id);
            await SecureStorageAccessor.SetVehicleId(selectedVehicle.Id);
        }
    }
}