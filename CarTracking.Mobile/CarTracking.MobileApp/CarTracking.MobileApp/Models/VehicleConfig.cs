namespace CarTracking.MobileApp.Models;

public class VehicleConfig
{
    public int LowBatteryThreshold { get; set; }
    public string Name { get; set; }
    public bool IsAssigned { get; set; }
    public string VehicleId { get; set; }
    public double VirtualGarageRadius { get; set; }
    public int HistoryInDays { get; set; }
}
