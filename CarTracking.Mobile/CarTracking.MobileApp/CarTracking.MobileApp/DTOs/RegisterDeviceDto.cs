namespace CarTracking.MobileApp.DTOs;

public class RegisterDeviceDto
{
    public bool IsAdminDevice { get; set; }
    public string? VehicleId { get; set; }
    public string NotificationToken { get; set; } = "empty";
}