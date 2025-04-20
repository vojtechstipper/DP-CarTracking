using CarTracking.MobileApp.DTOs;
using Refit;

namespace CarTracking.MobileApp.API;

public interface ICarTrackingApiUnathorized
{
    [Post("/users/login")]
    Task<ApiResponse<LoginDto>> LoginUser(LoginUserCommand loginUserCommand);   
    
    [Post("/users/reset-password")]
    Task<ApiResponse<string>> ResetPassword(ResetPasswordCommand loginUserCommand);    
    
    [Post("/users/set-new-password")]
    Task<ApiResponse<string>> SetNewPassword(SetNewPasswordCommand loginUserCommand);

    [Post("/users/register")]
    Task<ApiResponse<LoginDto>> RegisterUser(RegisterUserCommand registerUserCommand);
    
    [Post("/status")]
    Task<ApiResponse<string>> SendStatus(SendStatusDto statusDto);
}