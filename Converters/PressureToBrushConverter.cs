using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AeonHacs.Wpf.Converters
{
    [ValueConversion(typeof(double), typeof(Brush))]
	public class PressureToBrushConverter : IValueConverter
	{
		public static PressureToBrushConverter Default = new PressureToBrushConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var key = value is double pressure ?
				PressureToBrushResourceKeyConverter.BrushResourceKey(pressure) :
				PressureToBrushResourceKeyConverter.DefaultBrushResourceKey;
			var brush = (Brush)Application.Current.Resources[key];
			return brush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}