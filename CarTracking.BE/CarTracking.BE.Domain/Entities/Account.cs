namespace CarTracking.BE.Domain.Entities;

public class Account : Entity
{
    public IList<Device> Devices { get; set; }

    public IList<Vehicle> Vehicles { get; set; }

    public List<string> UserIds { get; set; }
    public string? Code { get; set; }
    public DateTime CodeValidTill { get; set; }
}