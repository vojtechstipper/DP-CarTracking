using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.Pages;

public partial class UserRegisterPage : ContentPage
{
    private readonly UserService _userService;

    public UserRegisterPage(UserService userService)
    {
        _userService = userService;
        InitializeComponent();
    }

    private async void OnRegisterButtonClicked(object? sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

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

        if (!IsPasswordValid(password))
        {
            ErrorMessage.Text = "Password must be at least 8 characters long and contain at least one number.";
            ErrorMessage.IsVisible = true;
            return;
        }

        var registrationResult = await _userService.RegisterUser(email, password);
        if (registrationResult.IsSuccess)
        {
            await Shell.Current.GoToAsync($"/{nameof(AccountAssigningPage)}");
        }
        else
        {
            ErrorMessage.Text = "Registration failed. Try again.";
            ErrorMessage.IsVisible = true;
        }
    }

    private bool IsPasswordValid(string password)
    {
        return password.Length >= 8 &&
               password.Any(char.IsDigit) &&
               password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}