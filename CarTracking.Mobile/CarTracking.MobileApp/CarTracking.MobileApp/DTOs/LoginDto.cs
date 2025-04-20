namespace CarTracking.MobileApp.DTOs;

public class LoginDto
{
    public required string Username { get; set; }
    public required string Token { get; set; }
    public bool IsAssignedToAccount { get; set; }
}