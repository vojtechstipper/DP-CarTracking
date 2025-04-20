namespace CarTracking.MobileApp.DTOs;

public class VehicleTripsListDto
{
    public required string VehicleId { get; set; }
    public ICollection<VehicleTripInfoDto> Trips { get; set; } = [];
}