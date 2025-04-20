namespace CarTracking.MobileApp.DTOs;

public class VehicleInfoListDto
{
    public List<VehicleListItemDto> Vehicles { get; set; } = [];
}

public class VehicleListItemDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public bool IsAssigned { get; set; }
    public LocationDto? LastKnownLocation { get; set; }
    public string? DeviceId { get; set; }
    public DateTime LastTimeSent { get; set; }
    public DateTime LastTimeMoved { get; set; }
}

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}