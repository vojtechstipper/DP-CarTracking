namespace CarTracking.BE.Domain.Entities;

public class VehicleSettings : Entity
{
    public int LowBatteryThreshold { get; set; } = 20;
    public double Radius { get; set; } = 50;
    public int HistoryLenghtInDays { get; set; } = 30;
}