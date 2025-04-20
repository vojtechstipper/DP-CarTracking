using System.Globalization;

namespace CarTracking.MobileApp.Converters;

public class BoolToChargingTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool)value! ? " (Charging)" : " (Discharging)";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}