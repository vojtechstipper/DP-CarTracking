using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using MyLocation = CarTracking.MobileApp.Models.Location;

namespace CarTracking.MobileApp.Helpers;

public static class MapHelper
{
    public static void MoveToPinLocation(this Map map, Pin pin) =>
        map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Location, Distance.FromMiles(1)));

    public static void AddPinToMap(this Map map, Pin pin) => map.Pins.Add(pin);

    public static Pin CreatePinForMap(this MyLocation locationDto, string vehicleName = "")
    {
        return new Pin
        {
            Label = vehicleName,
            Type = PinType.Place,
            Location = new Location(locationDto.Latitude, locationDto.Longitude)
        };
    }
}