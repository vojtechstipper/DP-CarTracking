namespace CarTracking.MobileApp.DTOs;

public class LoginUserCommand
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}