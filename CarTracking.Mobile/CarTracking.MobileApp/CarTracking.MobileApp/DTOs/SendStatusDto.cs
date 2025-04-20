namespace CarTracking.MobileApp.DTOs;

public class SendStatusDto
{
    
        public required string DeviceId { get; set; }
        public required string Status { get; set; }
        public DateTime Sent { get; set; }
        public required LocationDto Location { get; set; }
        public required BatteryInfoDto BatteryInfo { get; set; }

        public class LocationDto
        {
                public double Latitude { get; set; }
                public double Longitude { get; set; }
                public double? Speed { get; set; }
                public double? Accuracy { get; set; }
                public double? Altitude { get; set; }
        }
    
}