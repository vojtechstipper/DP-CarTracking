using CarTracking.MobileApp.Pages;
#if ANDROID
using Android.Content;
#endif

namespace CarTracking.MobileApp;

public partial class AppShell : Shell
{
    private MenuItem _settingsMenuItem;
    private MenuItem _adminOverview;
    private MenuItem _vehiclesManagement;
    private MenuItem _codeGenerator;
    private MenuItem _logoutMenuItem;
    private AppTheme _currentTheme = AppInfo.Current.RequestedTheme;

    public AppShell()
    {
        InitializeComponent();
#if ANDROID
        var mode = Preferences.Get("Mode", "");
        if (mode != "Admin")
        {
            var intent = new Intent(Android.App.Application.Context, typeof(ScreenOffService));
            intent.SetAction(ScreenOffService.ActionStartScreenOffService);
            Android.App.Application.Context.StartService(intent);
        }

#endif
        RegisterRoutes();

        InitializeMenu();
    }

    private void InitializeMenu()
    {
        var mode = Preferences.Get("Mode", "");
        if (!string.IsNullOrEmpty(mode) && mode == "Admin")
        {
            InitializeAdminMenu();
        }
        else
        {
            InitializeMonitorMenu();
        }
    }

    private void InitializeMonitorMenu()
    {
        AddLogoutMenuItem();
    }

    private void AddLogoutMenuItem()
    {
        _logoutMenuItem = new MenuItem
        {
            Text = "Logout",
            IconImageSource = DetermineImageSource("logout"),
            Command = new Command(DoLogout)
        };
        Items.Add(_logoutMenuItem);
    }

    private void InitializeAdminMenu()
    {
        AddAdminOverviewMenuItem();
        AddVehiclesManagementMenuItem();
        AddSettingsMenuItem();
        AddCodeGeneratorMenuItem();
        AddLogoutMenuItem();
    }

    private void AddSettingsMenuItem()
    {
        _settingsMenuItem = new MenuItem
        {
            Text = "Settings",
            IconImageSource = DetermineImageSource("cog"),
            Command = new Command(NavigateToSettings)
        };
        Items.Add(_settingsMenuItem);
    }

    private void AddVehiclesManagementMenuItem()
    {
        _vehiclesManagement = new MenuItem
        {
            Text = "Vehicles management",
            IconImageSource = DetermineImageSource("car_settings"),
            Command = new Command(NavigateToVehiclesManagement)
        };
        Items.Add(_vehiclesManagement);
    }

    private void AddAdminOverviewMenuItem()
    {
        _adminOverview = new MenuItem
        {
            Text = "Home",
            IconImageSource = DetermineImageSource("car_multiple"),
            Command = new Command(NavigateToAdminOverview)
        };
        Items.Add(_adminOverview);
    }

    private void AddCodeGeneratorMenuItem()
    {
        _codeGenerator = new MenuItem
        {
            Text = "Code for joining",
            IconImageSource = DetermineImageSource("account_key_outline"),
            Command = new Command(NavigateToCodeGenerator)
        };
        Items.Add(_codeGenerator);
    }

    private string DetermineImageSource(string imageName)
    {
        return _currentTheme == AppTheme.Dark ? $"{imageName}_dark.png" : $"{imageName}.png";
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(ModeSelectPage), typeof(ModeSelectPage));
        Routing.RegisterRoute(nameof(AdminModeHomePage), typeof(AdminModeHomePage));
        Routing.RegisterRoute(nameof(MonitorModeHomePage), typeof(MonitorModeHomePage));
        Routing.RegisterRoute(nameof(VehicleSelectPage), typeof(VehicleSelectPage));
        Routing.RegisterRoute(nameof(VehicleStatusDetailPage), typeof(VehicleStatusDetailPage));
        Routing.RegisterRoute(nameof(VehicleTripsPage), typeof(VehicleTripsPage));
        Routing.RegisterRoute(nameof(UserLoginPage), typeof(UserLoginPage));
        Routing.RegisterRoute(nameof(UserRegisterPage), typeof(UserRegisterPage));
        Routing.RegisterRoute(nameof(AccountAssigningPage), typeof(AccountAssigningPage));
        Routing.RegisterRoute(nameof(CreateVehiclePage), typeof(CreateVehiclePage));
        Routing.RegisterRoute(nameof(AccountCodeGeneratorPage), typeof(AccountCodeGeneratorPage));
        Routing.RegisterRoute(nameof(TripMapPage), typeof(TripMapPage));
        Routing.RegisterRoute(nameof(VehiclesManagementsPage), typeof(VehiclesManagementsPage));
        Routing.RegisterRoute(nameof(VehicleSettingsPage), typeof(VehicleSettingsPage));
        Routing.RegisterRoute(nameof(UserPasswordResetPage), typeof(UserPasswordResetPage));
        Routing.RegisterRoute(nameof(LocationSelectorPage), typeof(LocationSelectorPage));
    }

    private async void DoLogout()
    {
        Current.FlyoutIsPresented = false;
        Preferences.Clear();
        await Current.GoToAsync("//UserLogin");
    }

    private async void NavigateToSettings()
    {
        await DisplayAlert("Not Available", "Not implented yet", "OK");
    }

    private async void NavigateToVehiclesManagement()
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync($"/{nameof(VehiclesManagementsPage)}");
    }

    private async void NavigateToAdminOverview()
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync($"/{nameof(AdminModeHomePage)}");
    }

    private async void NavigateToCodeGenerator()
    {
        Current.FlyoutIsPresented = false;
        await Current.GoToAsync($"/{nameof(AccountCodeGeneratorPage)}");
    }
}