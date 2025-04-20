namespace CarTracking.BE.Domain.Entities;

public class Status : Entity
{
    public string DeviceId { get; set; }

    public Device Device { get; set; }

    public string VehicleId { get; set; }

    public Vehicle Vehicle { get; set; }

    public Location Location { get; set; }

    public BatteryInfo BatteryInfo { get; set; }

    public DateTime Sent { get; set; }

    public DateTime Received { get; set; }
    public DateTime StartTime { get; set; }
    public string? TripId { get; set; }
    public string? NamedVehicleLocationId { get; set; }
}