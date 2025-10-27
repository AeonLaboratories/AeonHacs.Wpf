using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace AeonHacs.Wpf.Converters;
public class TubingSizeConverter : TypeConverter
{
    public static double GetTubingSize(string input) => input switch
    {
        "1/8" or "3mm" => 2,
        "1/4" or "6mm" => 4,
        "3/8" or "9mm" => 6,
        "1/2" or "12mm" => 8,
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
            return GetTubingSize(input);
        }

        return base.ConvertFrom(context, culture, value);
    }
}
