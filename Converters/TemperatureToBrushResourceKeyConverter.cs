using System;
using System.Globalization;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(double), typeof(string))]
	public class TemperatureToBrushResourceKeyConverter : IValueConverter
	{
		public static TemperatureToBrushResourceKeyConverter Default = new TemperatureToBrushResourceKeyConverter();

		public static string DefaultBrushResourceKey = "UnknownBrush";
		public static string BrushResourceKey(double temperature)
		{
			if (temperature > 90)   // Do not touch!
				return "HotBrush";
			if (temperature > 40)   // Noticeably warm but not enough to cause a burn.
				return "WarmBrush";
			if (temperature < -300) // impossible by far enough to rule out any calibration error
				return "ErrorBrush";
			if (temperature < -100)
				return "ColdBrush";
			if (temperature < 10)
				return "CoolBrush";
			// between 10 and 40 °C
			return "NeutralBrush";
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is double temperature ? BrushResourceKey(temperature) : DefaultBrushResourceKey;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
