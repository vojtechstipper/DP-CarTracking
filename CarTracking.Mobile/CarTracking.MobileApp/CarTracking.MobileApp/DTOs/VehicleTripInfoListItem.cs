using MyLocation = CarTracking.MobileApp.Models.Location;

namespace CarTracking.MobileApp.DTOs;

public class VehicleTripInfoListItem
{
    public required string AddressFrom { get; set; }
    public required string AddressTo { get; set; }
    public DateTime TripStart { get; set; }
    public TimeSpan TripLenght { get; set; }
    public List<MyLocation> Locations { get; set; } = [];
    public string? TripId { get; set; }
}