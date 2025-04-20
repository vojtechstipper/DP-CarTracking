using CarTracking.BE.Application.DTOs;

namespace CarTracking.BE.Application.Vehicles.Queries.LastStatus;

public class VehicleStatusDto
{
    public string VehicleName { get; set; }
    public LocationDto Location { get; set; }
    public BatteryInfoDto BatteryInfo { get; set; }
    public DateTime Sent { get; set; }
    public SignalInfoDto SignalInfo { get; set; }
    public bool IsVirtualGarageActive { get; set; }
}