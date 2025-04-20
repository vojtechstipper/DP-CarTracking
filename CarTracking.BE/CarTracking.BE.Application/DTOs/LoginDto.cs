namespace CarTracking.BE.Application.DTOs;

public class LoginDto
{
    public string Token { get; set; }
    public string Username { get; set; }
    public bool IsAssignedToAccount { get; set; }
}