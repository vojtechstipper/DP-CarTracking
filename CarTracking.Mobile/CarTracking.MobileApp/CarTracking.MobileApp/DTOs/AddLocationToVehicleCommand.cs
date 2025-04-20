namespace CarTracking.MobileApp.DTOs;

public class AddLocationToVehicleCommand
{
    public LocationDto Location { get; set; }
    public string LocationName { get; set; }
    public int Radius { get; set; }
    public bool IsUsedForVirtualGarage { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
}