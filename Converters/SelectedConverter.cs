using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace HACS.WPF.Converters
{
	public class SelectedConverter : IValueConverter
	{
		public static SelectedConverter Default = new SelectedConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			value?.Equals(parameter) ?? parameter != null;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
