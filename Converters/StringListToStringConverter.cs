using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HACS.WPF.Converters
{
	public class StringListToStringConverter : IValueConverter
	{
		public static StringListToStringConverter Default = new StringListToStringConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is null)
				return "";
			return string.Join("\r\n", (value as List<string>).ToArray());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as string).Split("\r\n").ToList();
		}
	}
}
