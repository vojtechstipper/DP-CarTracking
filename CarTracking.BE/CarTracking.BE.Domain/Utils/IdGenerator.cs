namespace CarTracking.BE.Domain.Utils;

public class IdGenerator
{
    public static string GenerateId() => Guid.NewGuid().ToString("N");
}