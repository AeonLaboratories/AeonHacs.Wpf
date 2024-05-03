using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AeonHacs.Wpf.Converters;
public class CultureTubeSizeConverter : TypeConverter
{
    public static Size GetCultureTubeSize(string input) => input switch
    {
        "3mm" => new Size(2, 13),
        "6mm" => new Size(4, 17),
        "9mm" => new Size(6, 23),
        "12mm" => new Size(8, 25),
        "combustion" => new Size(6, 45),
        "ampoule" => new Size(4, 45),
        _ => Size.Parse(input)
    };

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DependencyProperty.UnsetValue;
            return GetCultureTubeSize(input);
        }

        return base.ConvertFrom(context, culture, value);
    }
}
