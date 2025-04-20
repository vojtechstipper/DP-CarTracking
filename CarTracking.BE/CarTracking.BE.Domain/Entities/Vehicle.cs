namespace CarTracking.BE.Domain.Entities;

public class Vehicle : Entity
{
    public string Name { get; set; }
    public VehicleSettings Settings { get; set; }
    public VirtualGarage? VirtualGarage { get; set; }
    public bool VirtualGarageIsEnabled { get; set; }
    public ICollection<Status> StatusHistory { get; set; }
    public ICollection<VehicleLocation> VehicleLocations { get; set; }
    public bool IsAssignedToDevice { get; set; }
    public string? DeviceId { get; set; }
    public Device? Device { get; set; }
    public string AccountId { get; set; }
    public Account Account { get; set; }
}