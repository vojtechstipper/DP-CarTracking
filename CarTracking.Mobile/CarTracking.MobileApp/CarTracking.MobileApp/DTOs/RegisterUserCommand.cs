namespace CarTracking.MobileApp.DTOs;

public class RegisterUserCommand
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}