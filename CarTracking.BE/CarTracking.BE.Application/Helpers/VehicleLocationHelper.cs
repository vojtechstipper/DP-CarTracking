using CarTracking.BE.Application.DTOs;
using CarTracking.BE.Domain.Entities;

namespace CarTracking.BE.Application.Helpers;

public static class VehicleLocationHelper
{
    public static VehicleLocation? FindNearestLocation(LocationDto location, List<VehicleLocation> locations)
    {
        var nearestLocation = locations
            .Select(loc => new
            {
                loc,
                Distance = Haversine(location.Latitude, location.Longitude, loc.Latitude,
                    loc.Longitude),
                loc.Radius
            })
            .Where(x => x.Distance <= x.Radius) // Filter locations within the radius
            .OrderBy(x => x.Distance) // Sort by closest first
            .FirstOrDefault();

        return nearestLocation?.loc;
    }

    private static double Haversine(double lat1, double lon1, double lat2, double lon2)
    {
        const double r = 6371000; // Radius of Earth in meters
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return r * c;
    }

    private static double ToRadians(double angle) => angle * Math.PI / 180;
}