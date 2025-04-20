namespace CarTracking.BE.Domain.Entities;

public class VehicleLocation : Entity
{
    public string VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int Radius { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Name { get; set; }
    public bool IsUsedForVirtualGarage { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
    
}