using CarTracking.MobileApp.API;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Helpers;
using CarTracking.MobileApp.Models;

namespace CarTracking.MobileApp.Services;

public class UserService(ApiWrapper<ICarTrackingApiUnathorized> carTrackingApi)
{
    public async Task<Result<LoginDto>> LoginUser(string username, string password)
    {
        var command = new LoginUserCommand()
        {
            Username = username,
            Password = password
        };
        var response = await carTrackingApi.ExecuteAsync(api => api.LoginUser(command));

        if (response.IsSuccess && string.IsNullOrEmpty(response.Value.Token)) return new Result<LoginDto>(false);

        if (!response.IsSuccess)
            return new Result<LoginDto>(false);

        await SecureStorageAccessor.SetToken(response.Value!.Token);

        Preferences.Set("IsLoggedIn", true);
        return new Result<LoginDto>(response.Value!, !string.IsNullOrEmpty(response.Value.Token));
    }

    public async Task<Result<LoginDto>> RegisterUser(string email, string password)
    {
        var command = new RegisterUserCommand()
        {
            Email = email,
            Password = password
        };
        var response = await carTrackingApi.ExecuteAsync(api => api.RegisterUser(command));

        if (!response.IsSuccess) return new Result<LoginDto>(false);

        await SecureStorageAccessor.SetToken(response.Value!.Token);
        Preferences.Set("IsLoggedIn", true);
        return new Result<LoginDto>(response.Value!, !string.IsNullOrEmpty(response.Value.Token));
    }
    
    public async Task<Result<string>> ResetPassword(string email)
    {
        var command = new ResetPasswordCommand()
        {
            Email = email
        };
        var response = await carTrackingApi.ExecuteAsync(api => api.ResetPassword(command));

        if (!response.IsSuccess) return new Result<string>(false);

        return new Result<string>(response.Value!, !string.IsNullOrEmpty(response.Value));
    } 
    
    public async Task<Result<string>> SetNewPassword(string email, string password, string code)
    {
        var command = new SetNewPasswordCommand()
        {
            Email = email,
            NewPassword = password,
            Code = code
        };
        var response = await carTrackingApi.ExecuteAsync(api => api.SetNewPassword(command));

        if (!response.IsSuccess) return new Result<string>(false);

        return new Result<string>(response.Value!, !string.IsNullOrEmpty(response.Value));
    }
}