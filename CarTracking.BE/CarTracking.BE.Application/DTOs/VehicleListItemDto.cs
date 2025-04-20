namespace CarTracking.BE.Application.DTOs;

public class VehicleListItemDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsAssigned { get; set; }
    public string? DeviceId { get; set; }
}