using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;
using System.Text;

namespace AeonHacs.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class ResourceKeyToBrushConverter : IValueConverter
    {
        public static ResourceKeyToBrushConverter Default = new ResourceKeyToBrushConverter();
        public static Brush DefaultBrush => Brushes.Transparent;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string key ? 
                (Brush)Application.Current.Resources[key] :
                DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
