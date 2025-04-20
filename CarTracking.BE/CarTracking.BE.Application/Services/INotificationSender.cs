using CarTracking.BE.Application.DTOs;

namespace CarTracking.BE.Application.Services;

public interface INotificationSender
{
    Task SendNotificationAsync(NotificationDto notification);
    Task SendNotificationsAsync(NotificationDto notification, List<string> tokens);
}