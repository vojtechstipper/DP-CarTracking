namespace CarTracking.BE.Application.DTOs;

public class VehicleTripsListDto
{
    public string VehicleId { get; set; }
    public ICollection<VehicleTripInfoDto> Trips { get; set; }
}