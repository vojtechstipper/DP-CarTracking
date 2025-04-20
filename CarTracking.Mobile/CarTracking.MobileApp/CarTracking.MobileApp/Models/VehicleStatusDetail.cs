namespace CarTracking.MobileApp.Models;

public class VehicleStatusDetail
{
    public string VehicleName { get; set; }
    public Location Location { get; set; }
    public BatteryInfo BatteryInfo { get; set; }
    public DateTime _sent { get; set; }
    public DateTime Sent
    {
        get => _sent.ToLocalTime(); // Converts UTC to local time
        set => _sent = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Ensure the value is treated as UTC
    }
    public SignalInfo SignalInfo { get; set; }
    public bool IsVirtualGarageActive { get; set; }
    public bool IsVirtualGarageActiveNeg
    {
        get => !IsVirtualGarageActive; }
}