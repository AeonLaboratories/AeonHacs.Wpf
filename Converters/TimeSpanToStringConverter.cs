using System;
using System.Globalization;
using System.Windows.Data;

namespace HACS.WPF.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
	public class TimeSpanToStringConverter : IValueConverter
	{
		public static TimeSpanToStringConverter Default = new TimeSpanToStringConverter();
		public const string DefaultValue = "23:59:59";
		public static string TimeText(TimeSpan ts) =>
			ts.ToString(ts.TotalDays < 1 ? @"h\:mm\:ss" : @"d\ h\:mm\:ss");

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is TimeSpan timeSpan ? TimeText(timeSpan) : DefaultValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
