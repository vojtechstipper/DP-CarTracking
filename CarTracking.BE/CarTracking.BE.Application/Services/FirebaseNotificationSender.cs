using CarTracking.BE.Application.DTOs;
using FirebaseAdmin.Messaging;

namespace CarTracking.BE.Application.Services;

public class FirebaseNotificationSender : INotificationSender
{
    public async Task SendNotificationAsync(NotificationDto notification)
    {
        var message = CreateNotificationMessage(notification);
        var messaging = FirebaseMessaging.DefaultInstance;
        try
        {
            var result = await messaging.SendAsync(message);
        }
        catch (Exception e)
        {
            
            Console.WriteLine("___Error sending message: ");
            Console.WriteLine(e);
        }
    }

    public async Task SendNotificationsAsync(NotificationDto notification, List<string> tokens)
    {
        List<Task> tasks = [];
        foreach (var token in tokens)
        {
            notification.Token = token;
            tasks.Add(SendNotificationAsync(notification));
        }

        await Task.WhenAll(tasks);
    }

    private Message CreateNotificationMessage(NotificationDto notificationDto)
    {
        return new Message()
        {
            Notification = new Notification
            {
                Title = notificationDto.Title,
                Body = notificationDto.Body,
            },
            Data = notificationDto.Data,
            Token = notificationDto.Token,
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                Notification = new AndroidNotification()
                {
                    VibrateTimingsMillis = [0, 500, 250, 500, 250, 500, 250, 750],
                    ChannelId = "com.companyname.cartracking.mobileapp.general",
                }
            }
        };
    }
}