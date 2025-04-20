using CarTracking.MobileApp.DTOs;
using Refit;

namespace CarTracking.MobileApp.API;

public interface ICarTrackingApi
{
    [Get("/vehicles")]
    Task<ApiResponse<VehiclesList>> GetVehicles();

    [Post("/vehicles")]
    Task<ApiResponse<string>> CreateVehicle(AddNewVehicleCommand newVehicle);

    [Get("/vehicles/basic-info")]
    Task<ApiResponse<VehicleInfoListDto>> GetVehiclesBasicInfo();

    [Get("/vehicles/{vehicleId}/last-status")]
    Task<ApiResponse<VehicleStatusDto>> GetVehicleLastStatus(string vehicleId);

    [Get("/vehicles/{vehicleId}/trips")]
    Task<ApiResponse<VehicleTripsListDto>> GetVehicleTrips(string vehicleId);

    [Get("/vehicles/{vehicleId}/trips/v2")]
    Task<ApiResponse<VehicleTripsListDto>> GetVehicleTripsV2(string vehicleId);

    [Post("/vehicles/{vehicleId}/trips/generate")]
    Task<ApiResponse<int>> GenerateTripsForVehicle(string vehicleId);

    [Delete("/vehicles/{vehicleId}/trips/{tripId}")]
    Task<ApiResponse<int>> DeleteTripsById(string vehicleId, string tripId);

    [Get("/vehicles/{vehicleId}/config")]
    Task<ApiResponse<VehicleConfigDto>> GetVehicleConfig(string vehicleId);

    [Put("/vehicles/{vehicleId}/config")]
    Task<ApiResponse<string>> EditVehicle(string vehicleId, EditVehicleCommand command);

    [Put("/vehicles/{vehicleId}/virtual-garage/disable")]
    Task<ApiResponse<string>> DisableVirtualGarage(string vehicleId);

    [Post("/vehicles/{vehicleId}/virtual-garage")]
    Task<ApiResponse<string>> EnableVirtualGarage(string vehicleId, EnableVirtualGarageCommand command);

    [Post("/vehicles/{vehicleId}/locations")]
    Task<ApiResponse<string>> AddLocationToVehicle(string vehicleId, AddLocationToVehicleCommand command);

    [Get("/vehicles/{vehicleId}/locations")]
    Task<ApiResponse<VehicleLocationsDto>> GetVehicleLocations(string vehicleId);

    [Delete("/vehicles/{vehicleId}/locations/{locationId}")]
    Task<ApiResponse<string>> DeleteVehicleLocation(string vehicleId, string locationId);
    
    [Put("/vehicles/{vehicleId}/locations/{locationId}")]
    Task<ApiResponse<string>> UpdateVehicleLocation(string vehicleId, string locationId,UpdateLocationToVehicleCommand command);

    [Post("/devices/register")]
    Task<ApiResponse<RegisteredDeviceDto>> RegisterDevice(RegisterDeviceDto registerDeviceDto); 
    
    [Post("/devices/{id}/test-notification")]
    Task<ApiResponse<string>> TestNotification(string id);

    [Post("/accounts/create")]
    Task<ApiResponse<CreateOrJoinAccountDto>> CreateNewAccount();

    [Post("/accounts/open")]
    Task<ApiResponse<OpenAccountDto>> OpenAccount();

    [Post("/accounts/join")]
    Task<ApiResponse<CreateOrJoinAccountDto>> JoinAccount(JoinAccountCommand joinAccountCommand);
}