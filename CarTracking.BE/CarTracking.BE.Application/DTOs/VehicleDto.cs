namespace CarTracking.BE.Application.DTOs;

public class VehicleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsAssigned { get; set; }
    public LocationDto LastKnownLocation { get; set; }
    public DateTime LastTimeSent { get; set; }
    public string? DeviceId { get; set; }
    public BatteryInfoDto BatteryInfo { get; set; }
    public DateTime LastTimeMoved { get; set; }
}