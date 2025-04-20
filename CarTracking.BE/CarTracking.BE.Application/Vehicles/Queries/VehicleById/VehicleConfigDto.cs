namespace CarTracking.BE.Application.Vehicles.Queries.VehicleById;

public class VehicleConfigDto
{
    public int LowBatteryThreshold { get; set; }
    public string Name { get; set; }
    public bool IsAssigned { get; set; }
    public double VirtualGarageRadius { get; set; }
    public int HistoryInDays { get; set; }
}