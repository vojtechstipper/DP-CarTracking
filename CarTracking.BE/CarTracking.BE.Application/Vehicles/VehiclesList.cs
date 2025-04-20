using CarTracking.BE.Application.DTOs;

namespace CarTracking.BE.Application.Vehicles;

public class VehiclesList
{
    public ICollection<VehicleListItemDto> Vehicles { get; set; }
}