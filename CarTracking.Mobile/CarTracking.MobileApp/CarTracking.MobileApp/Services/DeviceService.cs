using CarTracking.MobileApp.API;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.Models;
using Plugin.Firebase.CloudMessaging;

namespace CarTracking.MobileApp.Services;

public class DeviceService(ApiWrapper<ICarTrackingApi> carTrackingApi)
{
    public async Task<Result<RegisteredDeviceDto>> RegisterAdminDevice()
    {
        var token = await GetTokenResult();
        var request = new RegisterDeviceDto()
        {
            IsAdminDevice = true,
            VehicleId = null,
            NotificationToken = token
        };

        return await SendRegisterDeviceRequest(request);
    }

    public async Task<Result<RegisteredDeviceDto>> RegisterMonitorDevice(string vehicleId)
    {
        var request = new RegisterDeviceDto()
        {
            IsAdminDevice = false,
            VehicleId = vehicleId,
            NotificationToken = "token"
        };

        return await SendRegisterDeviceRequest(request);
    }

    private async Task<string> GetTokenResult()
    {
        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        return token;
    }

    private async Task<Result<RegisteredDeviceDto>> SendRegisterDeviceRequest(RegisterDeviceDto registerDeviceDto)
    {
        var response = await carTrackingApi.ExecuteAsync(api => api.RegisterDevice(registerDeviceDto));
        return response;
    }

    public async Task<Result<string>> SendTestNotification()
    {
        var deviceId = await SecureStorageAccessor.GetDeviceId();
        if (deviceId == null)
        {
            return new Result<string>(false);
        }

        var response = await carTrackingApi.ExecuteAsync(api => api.TestNotification(deviceId));
        return response;
    }
}