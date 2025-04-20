namespace CarTracking.MobileApp.DTOs;

public class EditVehicleCommand
{
    public int LowBatteryThreshold { get; set; }
    public string Name { get; set; }
    public double VirtualGarageRadius { get; set; }
    public int HistoryInDays { get; set; }
    
}