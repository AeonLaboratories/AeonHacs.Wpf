using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

[ValueConversion(typeof(object), typeof(string))]
public class ObjectToTypeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return DependencyProperty.UnsetValue;
        var type = value.GetType();
        if (typeof(IList).IsAssignableFrom(type) && type.IsGenericType)
            return type.GetGenericArguments()[0].Name.Plural();
        return type.Name;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}