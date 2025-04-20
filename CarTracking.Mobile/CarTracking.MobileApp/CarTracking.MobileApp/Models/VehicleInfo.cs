using System.Globalization;

namespace CarTracking.MobileApp.Models;

public class VehicleInfo
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public bool IsAssigned { get; set; }
    public Location? LastKnownLocation { get; set; }
    public string LocationAddress { get; set; } = "No address";
    public string? DeviceId { get; set; }

    public DateTime LastTimeMoved
    {
        get => _lastTimeMoved.ToLocalTime();
        set => _lastTimeMoved = DateTime.SpecifyKind(value, DateTimeKind.Utc); 
        
    }

    private DateTime _lastTimeMoved;
    public DateTime LastTimeSent
    {
        get => _lastTimeSent.ToLocalTime(); // Converts UTC to local time
        set => _lastTimeSent = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Ensure the value is treated as UTC
    }
    private DateTime _lastTimeSent;
    public string LastTimeSentFormatted => LastTimeSent.ToString("G", CultureInfo.CurrentCulture);
    public string LastTimeMovedFormatted => LastTimeMoved.ToString("G", CultureInfo.CurrentCulture);
}