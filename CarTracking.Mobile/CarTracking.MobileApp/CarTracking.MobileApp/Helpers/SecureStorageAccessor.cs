namespace CarTracking.MobileApp.Helpers;

public static class SecureStorageAccessor
{
    private const string AccountIdKey = "accountId";
    private const string VehicleIdKey = "vehicleId";
    private const string DeviceIdKey = "deviceId";
    private const string TokenKey = "user_token";

    public static async Task<string?> GetAccountId()
    {
        return await SecureStorage.Default.GetAsync(AccountIdKey);
    }

    public static async Task<string?> GetVehicelId()
    {
        return await SecureStorage.Default.GetAsync(VehicleIdKey);
    }

    public static async Task<string?> GetDeviceId()
    {
        return await SecureStorage.Default.GetAsync(DeviceIdKey);
    }

    public static async Task<string?> GetToken()
    {
        return await SecureStorage.Default.GetAsync(TokenKey);
    }

    public static async Task SetDeviceId(string deviceId)
    {
        await SecureStorage.Default.SetAsync(DeviceIdKey, deviceId);
    } 
    
    public static async Task SetVehicleId(string vehicleId)
    {
        await SecureStorage.Default.SetAsync(VehicleIdKey, vehicleId);
    }   
    
    public static async Task SetToken(string token)
    {
        await SecureStorage.Default.SetAsync(TokenKey, token);
    }
}