namespace CarTracking.BE.Domain.Entities;

public class BatteryInfo : Entity
{
    public double ChargeLevel { get; set; }
    public bool IsEnergySaverOn { get; set; }
    public bool IsCharging { get; set; }
}