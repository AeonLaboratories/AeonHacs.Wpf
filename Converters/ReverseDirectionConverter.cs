using AeonHacs.Wpf.Controls;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

[ValueConversion(typeof(RelativeDirection), typeof(RelativeDirection))]
public class ReverseDirectionConverter : IValueConverter
{
    public static ReverseDirectionConverter Default { get; } = new ReverseDirectionConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is RelativeDirection direction)
            return direction.Reverse();
        return RelativeDirection.Down;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Convert(value, targetType, parameter, culture);
    }
}
