using System.Globalization;

namespace CarTracking.MobileApp.Converters;

public class BoolToAssignedTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool)value! ? "Assigned to device" : "Not Assigned to device";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}