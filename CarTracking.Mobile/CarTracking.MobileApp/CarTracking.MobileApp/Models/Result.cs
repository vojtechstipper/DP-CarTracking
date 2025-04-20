namespace CarTracking.MobileApp.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }

    public Result(T value, bool isSuccess)
    {
        Value = value;
        IsSuccess = isSuccess;
    }
    
    public Result( bool isSuccess)
    {
        Value = default;
        IsSuccess = isSuccess;
    }
}