using CarTracking.MobileApp.ViewModels;

namespace CarTracking.MobileApp.Pages;

public partial class UserPasswordResetPage : ContentPage
{
    private readonly PasswordResetViewModel _viewModel;

    public UserPasswordResetPage(PasswordResetViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnResetPasswordClicked(object? sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = PasswordConfirmEntry.Text;

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            ErrorMessage.Text = "Please fill in all fields.";
            ErrorMessage.IsVisible = true;
            return;
        }

        if (password != confirmPassword)
        {
            ErrorMessage.Text = "Passwords do not match.";
            ErrorMessage.IsVisible = true;
            return;
        }

        var result = await _viewModel.ResetPassword();
        if (result.IsSuccess) await Shell.Current.GoToAsync($"/{nameof(UserLoginPage)}");
        else
        {
            ErrorMessage.Text = "Something went wrong. Try again.";
            ErrorMessage.IsVisible = true;
        }
    }
}