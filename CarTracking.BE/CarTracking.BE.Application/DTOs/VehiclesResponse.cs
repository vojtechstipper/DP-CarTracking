namespace CarTracking.BE.Application.DTOs;

public class VehiclesResponse
{
    public ICollection<VehicleDto> Vehicles { get; set; }
}