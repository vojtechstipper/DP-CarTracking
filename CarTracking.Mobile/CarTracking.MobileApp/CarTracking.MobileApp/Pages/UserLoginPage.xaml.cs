using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

public partial class UserLoginPage : ContentPage
{
    private readonly UserLoginViewModel _view;

    public UserLoginPage(UserLoginViewModel view)
    {
        InitializeComponent();
        _view = view;
        BindingContext = _view;
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }

    private async void OnLoginButtonClicked(object? sender, EventArgs e)
    {
        _view.IsBusy = true;
        PasswordEntry.IsEnabled = false;
        UsernameEntry.IsEnabled = false;

        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        var result = await _view.SendLoginRequest(username, password);
        _view.IsBusy = false;
        if (!result.IsSuccess)
        {
            PasswordEntry.IsEnabled = true;
            UsernameEntry.IsEnabled = true;
            await DisplayAlert("Bad login", "Wrong email or password", "OK");
        }
    }

    private async void OnCreateAccountButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(UserRegisterPage)}");
    }

    private async void OnResetPasswordClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(UserPasswordResetPage)}");
    }
}