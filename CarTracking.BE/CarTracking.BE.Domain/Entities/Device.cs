namespace CarTracking.BE.Domain.Entities;

public class Device : Entity
{
    public string? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public string AccountId { get; set; }
    public Account Account { get; set; }
    public bool IsAdminDevice { get; set; }
    public string NotificationToken { get; set; }
}