using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.CloudMessaging.EventArgs;

namespace CarTracking.MobileApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        CrossFirebaseCloudMessaging.Current.NotificationTapped += Current_NotificationTapped;
        MainPage = new AppShell();

        var isLoggedIn = Preferences.Get("IsLoggedIn", false);
        if (!isLoggedIn)
        {
            Shell.Current.GoToAsync("//UserLogin");
        }
        else
        {
            var isModeChoosen = Preferences.Get("IsModeChoosen", false);
            if (isModeChoosen)
            {
                var mode = Preferences.Get("Mode", "");
                switch (mode)
                {
                    case "Monitor":
                        Shell.Current.GoToAsync("//Monitor");
                        break;
                    case "Admin":
                        Shell.Current.GoToAsync("//Admin");
                        break;
                    default:
                        Shell.Current.GoToAsync("//MainPage");
                        break;
                }
            }
            else
            {
                Shell.Current.GoToAsync("//ModeSelectPage");
            }
        }
    }

    private void Current_NotificationTapped(object sender, FCMNotificationTappedEventArgs e)
    {
        try
        {
            var data = e.Notification.Data;
            //reading data from notiifcatin and opening the page
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception:>>" + ex);
        }
    }
}