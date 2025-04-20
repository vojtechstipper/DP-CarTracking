namespace CarTracking.MobileApp.DTOs;

public class VehicleTripInfoDto
{
    public required LocationDto LocationStart { get; set; }
    public string? LocationStartName { get; set; }
    public required LocationDto LocationFinish { get; set; }
    public string? LocationFinishName { get; set; }
    public DateTime TripStartTime { get; set; }
    public DateTime TripEndTime { get; set; }
    public TimeSpan TripLength { get; set; }
    public List<LocationDto> Locations { get; set; }
    public string? TripId { get; set; }
}