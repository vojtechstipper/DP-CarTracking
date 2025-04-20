namespace CarTracking.BE.Infrastructure.Exceptions;

public class NotFoundException: Exception
{
    public string Code { get; set; }
    public NotFoundException(string message, string code = "NotFoundError") : base(message)
    {
        Code = code;
    }
}