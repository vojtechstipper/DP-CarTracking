using CarTracking.MobileApp.API;

namespace CarTracking.MobileApp.Pages;

public partial class AccountCodeGeneratorPage : ContentPage
{
    private readonly ApiWrapper<ICarTrackingApi> _apiWrapper;

    public AccountCodeGeneratorPage(ApiWrapper<ICarTrackingApi> apiWrapper)
    {
        _apiWrapper = apiWrapper;
        InitializeComponent();
    }

    private async void OnGenerateCodeButtonClicked(object? sender, EventArgs e)
    {
        var result =
            await _apiWrapper.ExecuteAsync(api => api.OpenAccount());
        if (!result.IsSuccess || string.IsNullOrEmpty(result.Value.Code))
        {
            await DisplayAlert("Join Account Error", "Could not join account, try again", "OK");
        }
        else
        {
            GeneratedCodeLabel.Text = result.Value.Code;
        }
    }

    private async void OnLabelTapped(object? sender, TappedEventArgs e)
    {
        var label = (Label)sender;
        await Clipboard.SetTextAsync(label.Text);
        await DisplayAlert("Copied", "Text has been copied to clipboard.", "OK");
    }
}