namespace CarTracking.BE.Application.DTOs;

public class NotificationDto
{
    public string Title { get; set; }
    public string Body { get; set; }

    public Dictionary<string, string> Data { get; set; }
    public string Token { get; set; }
}