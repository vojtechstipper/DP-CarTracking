namespace CarTracking.MobileApp.DTOs;

public class VehicleConfigDto
{
    public int LowBatteryThreshold { get; set; }
    public double VirtualGarageRadius { get; set; }
    public required string Name { get; set; }
    public bool IsAssigned { get; set; }
    public int HistoryInDays { get; set; }
}