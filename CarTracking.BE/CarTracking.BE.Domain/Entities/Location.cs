namespace CarTracking.BE.Domain.Entities;

public class Location : Entity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Speed { get; set; }
    public double? Accuracy { get; set; }
    public double? Altitude { get; set; }
}