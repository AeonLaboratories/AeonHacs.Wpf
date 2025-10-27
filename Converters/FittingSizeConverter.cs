using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace AeonHacs.Wpf.Converters;
public class FittingSizeConverter : TypeConverter
{
    public static double GetFittingSize(string input) => input switch
    {
        "1/8" => 4,
        "1/4" => 6,
        "3/8" => 8,
        "1/2" => 10,
        _ => double.TryParse(input, out double size) ? size : double.NaN
    };

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DependencyProperty.UnsetValue;
            return GetFittingSize(input);
        }

        return base.ConvertFrom(context, culture, value);
    }
}
