using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public class StringListToStringConverter : IValueConverter
{
    public static readonly StringListToStringConverter Default = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return "";
        return string.Join("\r\n", [.. (value as List<string>)]);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as string).Split("\r\n").ToList();
    }
}
