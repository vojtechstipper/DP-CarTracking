using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoMapper;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Services;
using Location = CarTracking.MobileApp.Models.Location;

namespace CarTracking.MobileApp.ViewModels;

public class VehicleTripsModelView(VehicleService vehicleService, IMapper mapper) : INotifyPropertyChanged
{
    public ObservableCollection<VehicleTripInfoListItem> Trips { get; set; } = [];
    public string VehicleId { get; set; } = string.Empty;
    bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public async Task LoadTrips()
    {
        Trips.Clear();

        var tripsRequest = await vehicleService.GetVehicleTripsV2(VehicleId);
        if (tripsRequest.IsSuccess)
        {
            foreach (var trip in tripsRequest.Value!.Trips.ToList())
            {
                var mappedTrip = new VehicleTripInfoListItem
                {
                    AddressFrom = await GetAddressForStartLocation(trip),
                    AddressTo = await GetAddressForFinishLocation(trip),
                    TripStart = trip.TripStartTime,
                    TripLenght = trip.TripLength,
                    Locations = mapper.Map<List<Location>>(trip.Locations),
                    TripId = trip.TripId
                };
                Trips.Add(mappedTrip);
            }

            OnPropertyChanged(nameof(Trips));
        }
    }

    private async Task<string> GetAddressForFinishLocation(VehicleTripInfoDto trip)
    {
        if (trip.LocationFinishName is not null)
        {
            return trip.LocationFinishName;
        }

        return await vehicleService.GetAddressFromCoordinates(trip.LocationFinish.Latitude,
            trip.LocationFinish.Longitude);
    }

    private async Task<string> GetAddressForStartLocation(VehicleTripInfoDto trip)
    {
        if (trip.LocationStartName is not null)
        {
            return trip.LocationStartName;
        }

        return await vehicleService.GetAddressFromCoordinates(trip.LocationStart.Latitude,
            trip.LocationStart.Longitude);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task GenerateTrips()
    {
        var request = await vehicleService.GenerateVehicleTrips(VehicleId);
        if (request.IsSuccess)
        {
            await LoadTrips();
        }
    }

    public async Task DeleteTrip(string tripId)
    {
        var request = await vehicleService.DeleteTripForVehicle(VehicleId, tripId);
        if (request.IsSuccess)
        {
            await LoadTrips();
        }
    }
}