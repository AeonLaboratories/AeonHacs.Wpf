using AeonHacs.Wpf.Media;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Converters;

[ValueConversion(typeof(Brush), typeof(Color))]
public class DarkenConverter : IValueConverter
{
    public static readonly DarkenConverter Instance = new DarkenConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush && float.TryParse((string)parameter, out float amount))
        {
            return brush.Color.Darken((float)amount);
        }
        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
