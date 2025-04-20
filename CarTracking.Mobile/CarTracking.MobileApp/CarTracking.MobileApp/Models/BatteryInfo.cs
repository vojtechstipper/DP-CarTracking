namespace CarTracking.MobileApp.Models;

public class BatteryInfo
{
    public double ChargeLevel { get; set; }
    public double ChargeLevelInPercentage => Math.Round(ChargeLevel * 100, 0);
    public bool IsEnergySaverOn { get; set; }
    public bool IsCharging { get; set; }
}