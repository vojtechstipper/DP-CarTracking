namespace CarTracking.BE.Domain.Entities;

public class PasswordResetCode : Entity
{
    public string UserId { get; set; }
    public string Code { get; set; }
    public DateTime ValidTill { get; set; }
    public string Token { get; set; }
}