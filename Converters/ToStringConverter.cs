using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ToStringConverter : IValueConverter
    {
        public static ToStringConverter Default = new ToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
