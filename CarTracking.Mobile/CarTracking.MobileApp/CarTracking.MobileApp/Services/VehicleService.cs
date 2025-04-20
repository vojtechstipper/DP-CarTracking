using AutoMapper;
using CarTracking.MobileApp.API;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Models;

namespace CarTracking.MobileApp.Services;

public class VehicleService(ApiWrapper<ICarTrackingApi> carTrackingApi, IMapper mapper)
{
    public async Task<List<VehicleInfo>> GetVehiclesBasicInfo()
    {
        var response = await carTrackingApi.ExecuteAsync(api => api.GetVehiclesBasicInfo());
        if (!response.IsSuccess) return new();

        var vehicles = response.Value!;
        //namapovat
        var mappedVehicleInfos = mapper.Map<List<VehicleInfo>>(vehicles.Vehicles);
        foreach (var vehicle in mappedVehicleInfos)
        {
            if (vehicle.LastKnownLocation is not null)
                vehicle.LocationAddress = await GetAddressFromCoordinates(vehicle.LastKnownLocation.Latitude,
                    vehicle.LastKnownLocation.Longitude);
        }

        return mappedVehicleInfos;
    }

    public async Task<List<VehicleDto>> GetVehicles()
    {
        var response = await carTrackingApi.ExecuteAsync(api => api.GetVehicles());
        if (!response.IsSuccess) return new();

        return mapper.Map<List<VehicleDto>>(response.Value.Vehicles);
    }

    public async Task<VehicleConfig> GetVehicleConfig(string vehicleId)
    {
        var response = await carTrackingApi.ExecuteAsync(api => api.GetVehicleConfig(vehicleId));
        if (!response.IsSuccess) return new();

        return mapper.Map<VehicleConfig>(response.Value);
    }

    public async Task<string> GetAddressFromCoordinates(double latitude, double longitude)
    {
        try
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                return
                    $"{placemark.Thoroughfare} {placemark.Locality}, {placemark.SubAdminArea}, {placemark.CountryCode}, {placemark.PostalCode}";
            }

            return "No address found.";
        }
        catch (Exception ex)
        {
            // Handle exceptions (like no network, or invalid location)
            return $"Unable to get address: {ex.Message}";
        }
    }

    public async Task<Result<VehicleStatusDto>> GetVehicleLastStatus(string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api => api.GetVehicleLastStatus(vehicleId));
    }

    public async Task<Result<VehicleTripsListDto>> GetVehicleTrips(string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api => api.GetVehicleTrips(vehicleId));
    }

    public async Task<Result<VehicleTripsListDto>> GetVehicleTripsV2(string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api => api.GetVehicleTripsV2(vehicleId));
    }

    public async Task<Result<string>> CreateNewVehicle(string vehicleName)
    {
        return await carTrackingApi.ExecuteAsync(api => api.CreateVehicle(new AddNewVehicleCommand(vehicleName)));
    }

    public async Task<Result<string>> SaveEditedVehicle(VehicleConfig config)
    {
        return await carTrackingApi.ExecuteAsync(api => api.EditVehicle(config.VehicleId,
            new EditVehicleCommand()
            {
                Name = config.Name,
                LowBatteryThreshold = config.LowBatteryThreshold,
                VirtualGarageRadius = config.VirtualGarageRadius,
                HistoryInDays = config.HistoryInDays
            }));
    }


    public async Task<Result<string>> SetVirtualGarageForVehicle(string vehicleId, DateTime endOfValidity)
    {
        return await carTrackingApi.ExecuteAsync(api =>
            api.EnableVirtualGarage(vehicleId, new EnableVirtualGarageCommand() { EndOfValidity = endOfValidity }));
    }

    public async Task<Result<string>> DisableVirtualGarage(string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api =>
            api.DisableVirtualGarage(vehicleId));
    }

    public async Task<Result<int>> GenerateVehicleTrips(string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api =>
            api.GenerateTripsForVehicle(vehicleId));
    }

    public async Task<Result<int>> DeleteTripForVehicle(string vehicleId, string tripId)
    {
        return await carTrackingApi.ExecuteAsync(api =>
            api.DeleteTripsById(vehicleId, tripId));
    }

    public async Task<Result<string>> AddLocationToVehicle(string vehicleId, LocationDto locationDto,
        double currentRadius, string name, bool isUsedForVirtualGarage, TimeSpan fromTime, TimeSpan toTime)
    {
        return await carTrackingApi.ExecuteAsync(api => api.AddLocationToVehicle(vehicleId,
            new AddLocationToVehicleCommand()
            {
                Location = locationDto,
                Radius = (int)currentRadius,
                LocationName = name,
                IsUsedForVirtualGarage = isUsedForVirtualGarage,
                FromTime = fromTime,
                ToTime = toTime
            }));
    }

    public async Task<Result<VehicleLocationsDto>> GetVehicleLocations(string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api => api.GetVehicleLocations(vehicleId));
    }

    public async Task<Result<string>> DeleteVehicleLocation(string vehicleLocationId, string vehicleId)
    {
        return await carTrackingApi.ExecuteAsync(api => api.DeleteVehicleLocation(vehicleId, vehicleLocationId));
    }

    public async Task<Result<string>> UpdateLocationToVehicle(string vehicleId, LocationDto locationDto,
        double currentRadius, string locationNameText, string id, bool isUsedForVirtualGarage, TimeSpan fromTime,
        TimeSpan toTime)
    {
        return await carTrackingApi.ExecuteAsync(api => api.UpdateVehicleLocation(vehicleId, id,
            new UpdateLocationToVehicleCommand()
            {
                Location = locationDto,
                Radius = (int)currentRadius,
                LocationName = locationNameText,
                IsUsedForVirtualGarage = isUsedForVirtualGarage,
                FromTime = fromTime,
                ToTime = toTime
            }));
    }
}