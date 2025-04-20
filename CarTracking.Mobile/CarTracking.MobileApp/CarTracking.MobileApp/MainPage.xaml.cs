using CarTracking.MobileApp.Pages;

namespace CarTracking.MobileApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnChooseModeClicked(object? sender, EventArgs e)
    {
        var isModeChoosen = Preferences.Get("IsModeChoosen", false);
        if (!isModeChoosen)
        {
            await Shell.Current.GoToAsync($"/{nameof(ModeSelectPage)}");
        }
    }

    private void OnRemoveAllPreferencesClicked(object? sender, EventArgs e)
    {
        Preferences.Clear();
    }
}