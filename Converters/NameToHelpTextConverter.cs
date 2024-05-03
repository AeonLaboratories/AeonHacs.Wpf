using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

//TODO expand or replace this?
[ValueConversion(typeof(string), typeof(string))]
public class NameToHelpTextConverter : IValueConverter
{
    public static NameToHelpTextConverter Instance { get; } = new NameToHelpTextConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string name)
        {
            string type = name[0] switch
            {
                'v' => " valve",
                'h' => " heater",
                'p' => " pressure sensor",
                't' => " temperature sensor",
                _ => ""
            };
            if (name.StartsWith("ftc"))
                type = " coldfinger";

            name = name.TrimStart('v', 'h', 'p', 't');
            name = name.Replace("ftc", "");
            name = name.Replace('_', '-');
            name = name.Replace("Flow", " flow");

            return name + type;
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
