using CarTracking.MobileApp.API;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Helpers;

namespace CarTracking.MobileApp.Pages;

public partial class AccountAssigningPage : ContentPage
{
    private readonly ApiWrapper<ICarTrackingApi> _carTrackingApi;

    public AccountAssigningPage(ApiWrapper<ICarTrackingApi> carTrackingApi)
    {
        _carTrackingApi = carTrackingApi;
        InitializeComponent();
    }

    private async void OnCreateNewAccountButtonClicked(object? sender, EventArgs e)
    {
        var result = await _carTrackingApi.ExecuteAsync(api => api.CreateNewAccount());
        if (!result.IsSuccess || string.IsNullOrEmpty(result.Value.Token))
        {
            await DisplayAlert("Create Account Error", "Could not create new account", "OK");
        }
        else
        {
            await SecureStorageAccessor.SetToken(result.Value.Token);
            await Shell.Current.GoToAsync($"/{nameof(ModeSelectPage)}");
        }
    }

    private async void OnJoinAccountClicked(object? sender, EventArgs e)
    {
        string code = AccountEntry.Text;
        var result =
            await _carTrackingApi.ExecuteAsync(api => api.JoinAccount(new JoinAccountCommand() { Code = code }));
        if (!result.IsSuccess || string.IsNullOrEmpty(result.Value.Token))
        {
            await DisplayAlert("Join Account Error", "Could not join account, try again", "OK");
        }
        else
        {
            await SecureStorageAccessor.SetToken(result.Value.Token);
            await Shell.Current.GoToAsync($"/{nameof(ModeSelectPage)}");
        }
    }
}