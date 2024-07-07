using System;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class PressureToBrushResourceKeyConverter : IValueConverter
    {
        public static Components.IPressure Ambient;
        public static PressureToBrushResourceKeyConverter Default = new PressureToBrushResourceKeyConverter();

        public static string DefaultBrushResourceKey = "UnknownBrush";
        public static string BrushResourceKey(double pressure)
        {
            pressure = Math.Abs(pressure);
            if (pressure < 0.1)                                // Torr
                return "HighVacuumBrush";

            // get the ambient pressure
            if (Ambient == null)
            {
                try { Ambient = NamedObject.Find<Components.IPressure>("pAmbient"); }
                catch { }
            }
            double pAmbient = Ambient?.Pressure ?? 760;     // Torr

            return pressure >= pAmbient ? "GaugeBrush" : "VacuumBrush";
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double pressure ? BrushResourceKey(pressure) : DefaultBrushResourceKey;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}