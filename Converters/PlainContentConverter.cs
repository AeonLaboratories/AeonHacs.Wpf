using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public class PlainContentConverter : IValueConverter
{
    public static PlainContentConverter Default = new PlainContentConverter();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text)
        {
            if (text.IsBlank())
                return DependencyProperty.UnsetValue;
            return text.Replace("_", "__");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string text)
            return text.Replace("__", "_");
        return value;
    }
}
