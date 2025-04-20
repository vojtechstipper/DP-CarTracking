using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class AdminHomePageModelView : INotifyPropertyChanged
{
    private readonly VehicleService _vehicleService;
    public ObservableCollection<VehicleInfo> Vehicles { get; set; }
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
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
    }

    public Action<object, EventArgs> RefreshCompleted { get; set; }

    public AdminHomePageModelView(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
        Vehicles = new ObservableCollection<VehicleInfo>();
        RefreshCommand = new Command(OnRefresh);
    }

    private async void OnRefresh(object o)
    {
        IsRefreshing = true;

        await LoadVehicles();

        IsRefreshing = false;
        RefreshCompleted?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadVehicles()
    {
        var vehicles = await _vehicleService.GetVehiclesBasicInfo();

        Vehicles.Clear();
        vehicles.ForEach(vehicle => Vehicles.Add(vehicle));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}