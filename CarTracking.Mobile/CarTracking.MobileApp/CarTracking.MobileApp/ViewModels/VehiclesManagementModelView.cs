using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class VehiclesManagementModelView : INotifyPropertyChanged
{
    private readonly VehicleService _vehicleService;
    public ICommand RefreshCommand { get; }
    private bool isRefreshing;

    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            if (isRefreshing != value)
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
    }

    public Action<object, EventArgs> RefreshCompleted { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<VehicleDto> Vehicles { get; set; } = [];

    public VehiclesManagementModelView(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
        RefreshCommand = new Command(OnRefresh);
    }


    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task LoadVehicles()
    {
        var vehicles = await _vehicleService.GetVehicles();
        Vehicles.Clear();
        vehicles.ForEach(x => Vehicles.Add(x));
    }

    private async void OnRefresh(object o)
    {
        IsRefreshing = true;

        await LoadVehicles();

        IsRefreshing = false;
        RefreshCompleted?.Invoke(this, EventArgs.Empty);
    }
}