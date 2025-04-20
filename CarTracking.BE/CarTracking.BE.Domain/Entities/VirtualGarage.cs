namespace CarTracking.BE.Domain.Entities;

public class VirtualGarage : Entity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime ValidTill { get; set; } = DateTime.MinValue;
}