using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AeonHacs.Wpf.Converters;

[ValueConversion(typeof(object), typeof(Visibility))]
public class NullToVisibilityConverter : IValueConverter
{
    public static NullToVisibilityConverter Hidden = new NullToVisibilityConverter(Visibility.Hidden);
    public static NullToVisibilityConverter Collapsed = new NullToVisibilityConverter(Visibility.Collapsed);

    [ConstructorArgument("nullVisibility")]
    public Visibility NullVisibility { get; set; }

    public NullToVisibilityConverter()
    {
        NullVisibility = Visibility.Collapsed;
    }

    public NullToVisibilityConverter(Visibility nullVisibility)
    {
        NullVisibility = nullVisibility;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return NullVisibility;
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
