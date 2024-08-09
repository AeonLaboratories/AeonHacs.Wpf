using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters;

public enum PressureBin
{
    /// <summary>
    /// Above atmospheric pressure.
    /// </summary>
    [Description("Above atmospheric pressure.")]
    Gauge,
    /// <summary>
    /// Below atmosphere but above HighVacuum.
    /// </summary>
    [Description("Below atmosphere but above HighVacuum.")]
    Vacuum,
    /// <summary>
    /// Below 0.1 Torr. (Not really HighVacuum but significantly low (~1/7600 atm.)
    /// </summary>
    [Description("Below 0.1 Torr")]
    HighVacuum
}


[ValueConversion(typeof(double), typeof(PressureBin))]
public class PressureBinConverter : IValueConverter
{
    public static readonly PressureBinConverter Default = new();
    static Components.IPressure Ambient;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // get the ambient pressure
        if (Ambient == null)
        {
            try { Ambient = NamedObject.Find<Components.IPressure>("pAmbient"); }
            catch { }
        }
        double pAmbient = Ambient?.Pressure ?? 760;     // Torr

        var pressure = (double)value; 
        if (pressure < 0) pressure = 0;

        return pressure < 0.1 ? PressureBin.HighVacuum :
            pressure > pAmbient ? PressureBin.Gauge :
            PressureBin.Vacuum;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
