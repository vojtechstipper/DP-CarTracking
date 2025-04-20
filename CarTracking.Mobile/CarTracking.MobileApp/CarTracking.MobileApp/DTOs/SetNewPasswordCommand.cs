namespace CarTracking.MobileApp.DTOs;

public class SetNewPasswordCommand
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string Code { get; set; }
}