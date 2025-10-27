using System;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public class FittingToTubingSizeConverter : IValueConverter
{
    public static readonly FittingToTubingSizeConverter Instance = new FittingToTubingSizeConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double fittingSize)
            return fittingSize - 2;
        return 4;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double tubingSize)
            return tubingSize + 2;
        return 6;
    }
}
