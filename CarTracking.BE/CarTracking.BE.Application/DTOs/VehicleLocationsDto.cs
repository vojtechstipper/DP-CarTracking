namespace CarTracking.BE.Application.DTOs;

public class VehicleLocationsDto
{
    public string VehicleId { get; set; }
    public List<VehicleLocationDto> Locations { get; set; }
}

public class VehicleLocationDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Radius { get; set; }
    public LocationDto Location { get; set; }
    public bool IsUsedForVirtualGarage { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}