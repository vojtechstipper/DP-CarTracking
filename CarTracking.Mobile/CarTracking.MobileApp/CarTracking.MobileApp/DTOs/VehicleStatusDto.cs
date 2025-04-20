namespace CarTracking.MobileApp.DTOs;

public class VehicleStatusDto
{
    public string VehicleName { get; set; }
    public LocationDto Location { get; set; }
    public BatteryInfoDto BatteryInfo { get; set; }
    public DateTime Sent { get; set; }
    public SignalInfoDto SignalInfo { get; set; }
    public bool IsVirtualGarageActive { get; set; }
    public DateTime LastTimeMoved { get; set; }
}