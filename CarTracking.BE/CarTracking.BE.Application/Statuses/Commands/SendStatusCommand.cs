using MediatR;

namespace CarTracking.BE.Application.Status.Commands;

public class SendStatusCommand : IRequest<Unit>
{
    public string DeviceId { get; set; }
    public string Status { get; set; }
    public LocationDto Location { get; set; }
    public BatteryInfoDto BatteryInfo { get; set; }
    public DateTime Sent { get; set; }

    public class LocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Speed { get; set; }
        public double? Accuracy { get; set; }
        public double? Altitude { get; set; }
    }

    public class BatteryInfoDto
    {
        public double ChargeLevel { get; set; }
        public bool IsEnergySaverOn { get; set; }
        public bool IsCharging { get; set; }
    }
}