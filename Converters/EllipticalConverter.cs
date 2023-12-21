using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AeonHacs.Wpf.Converters
{
	public class EllipticalConverter : IValueConverter
	{
		public static EllipticalConverter Default = new EllipticalConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
				return new CornerRadius(double.MaxValue);
			else
				return new CornerRadius(0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
