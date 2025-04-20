using System.ComponentModel;
using System.Windows.Input;
using AutoMapper;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class VehicleStatusDetailModelView : INotifyPropertyChanged
{
    private VehicleStatusDetail _lastStatus;
    bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }

    public VehicleStatusDetail LastStatus
    {
        get => _lastStatus;
        set
        {
            if (_lastStatus != value)
            {
                _lastStatus = value;
                OnPropertyChanged(nameof(LastStatus));
            }
        }
    }

    private string _locationAddress;
    public DateTime VirtualGarageMinDate { get; set; } = DateTime.Now;

    public string LocationAddress
    {
        get => _locationAddress;
        set
        {
            if (_locationAddress != value)
            {
                _locationAddress = value;
                OnPropertyChanged(nameof(LocationAddress));
            }
        }
    }

    public string VehicleId { get; set; }

    public DateTime VirtualGarageEndDate { get; set; } = DateTime.Now;
    public TimeSpan VirtualGarageEndTime { get; set; }
    public ICommand RefreshCommand { get; set; }

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

    private bool isRefreshing;
    private readonly VehicleService _vehicleService;
    private readonly IMapper _mapper;

    public VehicleStatusDetailModelView(VehicleService vehicleService, IMapper mapper)
    {
        RefreshCommand = new Command(OnRefresh);
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    private async void OnRefresh(object o)
    {
        IsRefreshing = true;

        await LoadVehicleLastStatus();

        IsRefreshing = false;
    }

    public async Task LoadVehicleLastStatus()
    {
        IsBusy = true;
        var result = await _vehicleService.GetVehicleLastStatus(VehicleId);
        if (result.IsSuccess)
        {
            LastStatus = _mapper.Map<VehicleStatusDetail>(result.Value);
            LocationAddress =
                await _vehicleService.GetAddressFromCoordinates(LastStatus.Location.Latitude,
                    LastStatus.Location.Longitude);
        }

        IsBusy = false;
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task DeactivateVirtualGarage()
    {
        await _vehicleService.DisableVirtualGarage(VehicleId);
        await LoadVehicleLastStatus();
    }

    public async Task ActivateVirtualGarage()
    {
        var endOfValidity = VirtualGarageEndDate.Date.Add(VirtualGarageEndTime);
        await _vehicleService.SetVirtualGarageForVehicle(VehicleId, endOfValidity);
        await LoadVehicleLastStatus();
    }
}