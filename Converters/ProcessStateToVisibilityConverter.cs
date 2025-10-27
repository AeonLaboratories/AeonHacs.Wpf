using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public class ProcessStateToVisibilityConverter : IValueConverter
{
    public enum NotReadyVisibilityBehavior { Collapsed, Hidden }

    public static ProcessStateToVisibilityConverter Default = new ProcessStateToVisibilityConverter();
    public static ProcessStateToVisibilityConverter Hidden = new ProcessStateToVisibilityConverter() { NotReadyVisibility = NotReadyVisibilityBehavior.Hidden };

    public NotReadyVisibilityBehavior NotReadyVisibility { get; set; } = NotReadyVisibilityBehavior.Collapsed;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Components.ProcessManager.ProcessStateCode state && state == Components.ProcessManager.ProcessStateCode.Ready)
            return Visibility.Visible;
        return NotReadyVisibility == NotReadyVisibilityBehavior.Hidden ? Visibility.Hidden : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
