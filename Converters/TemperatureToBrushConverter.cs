using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Converters
{
	[ValueConversion(typeof(double), typeof(Brush))]
	public class TemperatureToBrushConverter : IValueConverter
	{
		public static TemperatureToBrushConverter Default = new TemperatureToBrushConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var key = value is double Temperature ?
				TemperatureToBrushResourceKeyConverter.BrushResourceKey(Temperature) :
				TemperatureToBrushResourceKeyConverter.DefaultBrushResourceKey;
			var brush = (Brush)Application.Current.Resources[key];
			return brush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}