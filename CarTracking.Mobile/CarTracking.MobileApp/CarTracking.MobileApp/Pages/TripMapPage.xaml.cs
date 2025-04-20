using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using CarTrackingLocation = CarTracking.MobileApp.Models.Location;

namespace CarTracking.MobileApp.Pages;

[QueryProperty(nameof(Locations), "Locations")]
public partial class TripMapPage : ContentPage
{
    public List<CarTrackingLocation> Locations
    {
        get => _locations;
        set
        {
            _locations = value;
            OnPropertyChanged();
        }
    }


    private List<CarTrackingLocation> _locations;

    public TripMapPage()
    {
        InitializeComponent();
        Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        if(map.MapElements.Any()) map.MapElements.Clear();
        AddPolyLinesToMap(Locations);
    }

    private void AddPolyLinesToMap(List<CarTrackingLocation>? locations)
    {
        if (locations is null) return;
        var polylines = new Polyline
        {
            StrokeColor = Colors.Red,
            StrokeWidth = 12,
        };

        foreach (var route in locations)
        {
            polylines.Geopath.Add(new Location(route.Latitude, route.Longitude));
        }

        map.MapElements.Add(polylines);

        MoveToBoundingRegion(locations);
    }

    private void MoveToBoundingRegion(List<CarTrackingLocation> positions)
    {
        if (positions == null || positions.Count == 0)
            return;

        double minLat = positions.Min(p => p.Latitude);
        double maxLat = positions.Max(p => p.Latitude);
        double minLng = positions.Min(p => p.Longitude);
        double maxLng = positions.Max(p => p.Longitude);

        // Calculate the center of the bounding box
        var centerLatitude = (minLat + maxLat) / 2;
        var centerLongitude = (minLng + maxLng) / 2;

        // Calculate the distance between the farthest points
        var latDifference = maxLat - minLat;
        var lngDifference = maxLng - minLng;

        // Adjust the map region
        var mapSpan = MapSpan.FromCenterAndRadius(
            new Location(centerLatitude, centerLongitude),
            Distance.FromKilometers(Math.Max(latDifference, lngDifference) * 111)); // Approx 1 degree ≈ 111km

        map.MoveToRegion(mapSpan);
    }
}