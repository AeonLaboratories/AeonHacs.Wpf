using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public enum TemperatureBin
{
    /// <summary>
    /// Safe to touch.
    /// </summary>
    [Description("Safe to touch.")]
    Neutral,
    /// <summary>
    /// Dangerously cold.
    /// </summary>
    [Description("Dangerously cold.")]
    Cold,
    /// <summary>
    /// Bit nippy out innit?
    /// </summary>
    [Description("Bit nippy out innit?")]
    Cool,
    /// <summary>
    /// Noticeably warm but not enough to cause a burn.
    /// </summary>
    [Description("Noticeably warm but not enough to cause a burn.")]
    Warm,
    /// <summary>
    /// Do not touch!
    /// </summary>
    [Description("Do not touch!")]
    Hot,
    /// <summary>
    /// Impossible by far enough to rule out any calibration error.
    /// </summary>
    [Description("Invalid reading. Sensor broken/missing?")]
    Error
}


[ValueConversion(typeof(double), typeof(TemperatureBin))]
public class TemperatureBinConverter : IValueConverter
{
    public static readonly TemperatureBinConverter Default = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (double)value switch
        {
            > 90 => TemperatureBin.Hot,
            > 40 => TemperatureBin.Warm,
            < -300 => TemperatureBin.Error,
            < -100 => TemperatureBin.Cold,
            < 10 => TemperatureBin.Cool,
            _ => TemperatureBin.Neutral
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
