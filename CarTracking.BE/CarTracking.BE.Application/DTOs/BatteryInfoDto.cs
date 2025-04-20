namespace CarTracking.BE.Application.DTOs;

public class BatteryInfoDto
{
    public double ChargeLevel { get; set; }
    public bool IsEnergySaverOn { get; set; }
    public bool IsCharging { get; set; }
}