using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public static BoolToVisibilityConverter Default = new BoolToVisibilityConverter();
    public static BoolToVisibilityConverter Hidden = new BoolToVisibilityConverter() { FalseVisibility = Visibility.Hidden };

    public Visibility TrueVisibility { get; set; } = Visibility.Visible;

    public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b && b)
            return TrueVisibility;
        return FalseVisibility;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
