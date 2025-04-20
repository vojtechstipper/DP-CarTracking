namespace CarTracking.BE.Application.Helpers;

public static class DateTimeConverterHelper
{
    public static DateTime ConvertToTimeZone(DateTime timeToConvert, TimeZoneInfo zoneInfo)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(timeToConvert, zoneInfo);
    }

    public static DateTime ConvertToCentralEuropeTimeZone(DateTime timeToConvert)
    {
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
        return ConvertToTimeZone(timeToConvert, timezone);
    }
}