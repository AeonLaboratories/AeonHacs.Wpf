using AeonHacs.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AeonHacs.Wpf.Converters;

[ValueConversion(typeof(RelativeDirection), typeof(RelativeDirection))]
public class InverseOrientationConverter : IValueConverter
{
    public static RelativeDirection Inverse(RelativeDirection input) => input switch
    {
        RelativeDirection.Left => RelativeDirection.Right,
        RelativeDirection.Up => RelativeDirection.Down,
        RelativeDirection.Right => RelativeDirection.Left,
        RelativeDirection.Down => RelativeDirection.Up,
        _ => throw new ArgumentException("Expected type of RelativeDirection")
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Inverse((RelativeDirection)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Inverse((RelativeDirection)value);
    }
}
