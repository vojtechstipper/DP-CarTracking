namespace CarTracking.MobileApp.DTOs;

public class VehiclesList
{
    public List<VehicleDto> Vehicles { get; set; } = [];
}

public class VehicleDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required bool IsAssigned { get; set; }
    public string? DeviceId { get; set; }
}